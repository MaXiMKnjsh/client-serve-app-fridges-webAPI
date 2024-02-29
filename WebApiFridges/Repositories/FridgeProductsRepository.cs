﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiFridges.Data;
using WebApiFridges.Models;
using WebApiFridges.MyIntefaces;
using WebApiFridges.MyResponceClasses;

namespace WebApiFridges.Repository
{
    public class FridgeProductsRepository : IFridgeProductsRepository
    {
        private readonly DataContext dataContext;
        public FridgeProductsRepository(DataContext dataCotnext)
        {
            this.dataContext = dataCotnext;
        }

        public IEnumerable<ResponceFridgeProducts> GetProductsList(Guid fridgeId)
        {
            IEnumerable<ResponceFridgeProducts> responce =
                from frPr in dataContext.FridgeProducts.Where(x => x.FridgeId == fridgeId)
                join pr in dataContext.Products
                on frPr.ProductId equals pr.Id
                select new ResponceFridgeProducts
                {
                    ProductGuid = frPr.ProductId,
                    Name = pr.Name,
                    Quantity = frPr.Quantity
                };

            return responce;
        }
        public bool AddProducts(Guid fridgeId, Guid productId, int quantity)
        {
            var newFridgeProducts = new FridgeProducts()
            {
                ProductId = productId,
                FridgeId = fridgeId,
                Quantity = quantity
            };

            dataContext.Add(newFridgeProducts);

            return Save();
        }

        public bool Save()
        {
            int savedCount = dataContext.SaveChanges();
            return savedCount > 0 ? true : false;
        }

        public bool IsFridgeExist(Guid fridgeId)
        {
            if (dataContext.Fridges.FirstOrDefault(x => x.Id == fridgeId) == null)
                return false;

            return true;
        }
		public bool AddProducts(IEnumerable<Guid> requestProducts, Guid fridgeGuid)
		{
			foreach (var productGuid in requestProducts)
			{
				dataContext.Add(new FridgeProducts()
				{
					ProductId = productGuid,
					FridgeId = fridgeGuid,
					Quantity = dataContext.Products.FirstOrDefault(x => x.Id == productGuid)?.DefaultQuantity ?? 0
				});
			}

			return Save();
		}

		public bool IsProductExist(Guid productId)
        {
            if (dataContext.Products.FirstOrDefault(x => x.Id == productId) == null)
                return false;

            return true;
        }
		public bool IsProducstExist(IEnumerable<Guid> productsGuids)
		{
			foreach (var i in productsGuids)
			{
				if (dataContext.Products.FirstOrDefault(x => x.Id == i) == null)
					return false;
			}
			return true;
		}

		public bool IsFridgeProductExist(Guid fridgeProductId)
        {
            if (dataContext.FridgeProducts.FirstOrDefault(x => x.Id == fridgeProductId) == null)
                return false;

            return true;
        }

        public bool RemoveFridgeProducts(Guid guid)
        {
            var productToRemove = dataContext.FridgeProducts.FirstOrDefault(x => x.Id == guid);
            
            if (productToRemove != null)
            dataContext.Remove(productToRemove);

            return Save();
        }

        public int UpdateProductsToDefQuant()
        {
            var fridgeProductsToUpdate = dataContext.FridgeProducts.Where(x => x.Quantity == 0).ToList();

            foreach (var i in fridgeProductsToUpdate)
            {
                // если дефолтного значения нет, то оставляется 0
                i.Quantity = dataContext.Products.FirstOrDefault(x => x.Id == i.ProductId)?.DefaultQuantity ?? 0;
            }

            return SaveWithCount(); // возвращает количество обновлённых продуктов в холодильнике
        }

        public int SaveWithCount()
        {
            return dataContext.SaveChanges();
        }

		public bool EditProducts(IEnumerable<Guid> productsGuids, Guid fridgeGuid)
		{
            var productsToDelete = dataContext.FridgeProducts.Where(x => x.FridgeId == fridgeGuid).ToList();

            dataContext.FridgeProducts.RemoveRange(productsToDelete);

            return AddProducts(productsGuids,fridgeGuid);
		}
	}
}
