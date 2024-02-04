﻿using System.Collections;
using WebApiFridges.Models;
using WebApiFridges.MyResponceClasses;

namespace WebApiFridges.MyIntefaces
{
    public interface IFridgeRepository   {
        IEnumerable<ResponceFridges> GetFridgesList();
        bool DeleteFridge(Guid guid);
        bool Save();
        bool isFridgeExist(Guid guid);
    }
}
