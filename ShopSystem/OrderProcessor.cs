using System;
using System.Collections.Generic;
using System.Text;

namespace ShopSystem
{
    public class OrderProcessor
    {
        private readonly IInventoryService _inventoryService;
        private readonly DiscountCalculator _discountCalculator;

        public OrderProcessor(IInventoryService inventoryService, DiscountCalculator discountCalculator)
        {
            _inventoryService = inventoryService;
            _discountCalculator = discountCalculator;
        }

        public decimal ProcessOrder(int productId, int quantity, decimal pricePerItem)
        {
            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть больше нуля.");

            if (!_inventoryService.CheckStock(productId, quantity))
                throw new InvalidOperationException("Недостаточно товара на складе.");

            decimal total = quantity * pricePerItem;
            decimal discount = _discountCalculator.CalculateDiscount(total);

            return total - discount;
        }
    }
}