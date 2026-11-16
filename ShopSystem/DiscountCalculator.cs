using System;
using System.Collections.Generic;
using System.Text;

namespace ShopSystem
{
    public class DiscountCalculator
    {
        public decimal CalculateDiscount(decimal totalAmount)
        {
            if (totalAmount > 10000)
                return totalAmount * 0.15m; // 15% скидка
            else if (totalAmount > 5000)
                return totalAmount * 0.10m; // 10% скидка
            else
                return 0;
        }
    }
}