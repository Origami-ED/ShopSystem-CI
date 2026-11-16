using System;
using System.Collections.Generic;
using System.Text;

namespace ShopSystem
{
    public interface IInventoryService
    {
        bool CheckStock(int productId, int quantity);
    }
}