﻿using WebApiFridges.Data;
using WebApiFridges.Models;
using WebApiFridges.MyIntefaces;

namespace WebApiFridges.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext dataContext;
        public ProductRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public bool IsProductExist(Guid productId)
        {
            if (dataContext.Products.FirstOrDefault(x => x.Id == productId) == null)
                return false;

            return true;
        }

        public bool ProductUpdate(Guid productId, string newProductName, int newDefQuantity)
        {
            Product? product = dataContext.Products.FirstOrDefault(x => x.Id == productId);

            if (product != null)
            {
                product.Name = newProductName;
                product.DefaultQuantity = newDefQuantity;
            }

            return Save();
        }

        public bool Save()
        {
            int savedCount = dataContext.SaveChanges();
            return savedCount > 0 ? true : false;
        }
    }
}
