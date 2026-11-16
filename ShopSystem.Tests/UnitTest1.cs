using Xunit;
using Moq;
using ShopSystem;
using System;

namespace ShopSystem.Tests
{
    public class OrderProcessorTests
    {
        [Fact]
        public void ProcessOrder_QuantityIsZero_ThrowsArgumentException()
        {
            // Arrange (Подготовка)
            var mockInventory = new Mock<IInventoryService>();
            var discountCalc = new DiscountCalculator();
            var processor = new OrderProcessor(mockInventory.Object, discountCalc);

            // Act & Assert (Действие и проверка)
            var exception = Assert.Throws<ArgumentException>(() => processor.ProcessOrder(1, 0, 100));
            Assert.Contains("больше нуля", exception.Message);
        }

        [Fact]
        public void ProcessOrder_NotEnoughStock_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockInventory = new Mock<IInventoryService>();
            // Настраиваем мок так, чтобы он возвращал false (товара нет)
            mockInventory.Setup(m => m.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

            var discountCalc = new DiscountCalculator();
            var processor = new OrderProcessor(mockInventory.Object, discountCalc);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => processor.ProcessOrder(1, 5, 100));
            Assert.Contains("Недостаточно товара", exception.Message);
        }

        [Fact]
        public void ProcessOrder_OrderOver10000_Applies15PercentDiscount()
        {
            // Arrange
            var mockInventory = new Mock<IInventoryService>();
            mockInventory.Setup(m => m.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var discountCalc = new DiscountCalculator();
            var processor = new OrderProcessor(mockInventory.Object, discountCalc);

            // Act: 10 штук по 1500 = 15000. Скидка 15% = 2250. Итог = 12750.
            var result = processor.ProcessOrder(1, 10, 1500);

            // Assert
            Assert.Equal(12750m, result);
        }

        [Fact]
        public void ProcessOrder_OrderOver5000_Applies10PercentDiscount()
        {
            // Arrange
            var mockInventory = new Mock<IInventoryService>();
            mockInventory.Setup(m => m.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var discountCalc = new DiscountCalculator();
            var processor = new OrderProcessor(mockInventory.Object, discountCalc);

            // Act: 10 штук по 600 = 6000. Скидка 10% = 600. Итог = 5400.
            var result = processor.ProcessOrder(1, 10, 600);

            // Assert
            Assert.Equal(5400m, result);
        }

        [Fact]
        public void ProcessOrder_OrderUnder5000_NoDiscount()
        {
            // Arrange
            var mockInventory = new Mock<IInventoryService>();
            mockInventory.Setup(m => m.CheckStock(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            var discountCalc = new DiscountCalculator();
            var processor = new OrderProcessor(mockInventory.Object, discountCalc);

            // Act: 2 штуки по 1000 = 2000. Скидки нет.
            var result = processor.ProcessOrder(1, 2, 1000);

            // Assert
            Assert.Equal(2000m, result);
        }
        [Fact]
        public void Product_Properties_SetAndGetCorrectly()
        {
            // Arrange
            var product = new Product();
            var expectedName = "Ноутбук";
            var expectedPrice = 50000m;
            var expectedStock = 10;

            // Act
            product.Name = expectedName;
            product.Price = expectedPrice;
            product.Stock = expectedStock;

            // Assert
            Assert.Equal(expectedName, product.Name);
            Assert.Equal(expectedPrice, product.Price);
            Assert.Equal(expectedStock, product.Stock);
        }
    }

}