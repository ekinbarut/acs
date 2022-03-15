using System;
using System.Collections.Generic;
using Albelli.Abstraction.Enum;
using Albelli.Domain.Entity;
using Moq;
using Xunit;

namespace Albelli.Tests.EntityTests
{
    public class OrderTests
    {
        private Mock<Order> mockOrder;

        private static Dictionary<string, double> productWidth = new Dictionary<string, double>()
        {
            {ProductType.Calendar.ToString(), 10},
            {ProductType.PhotoBook.ToString(), 19},
            {ProductType.Canvas.ToString(), 16},
            {ProductType.Cards.ToString(), 4.7},
            {ProductType.Mug.ToString(), 94}
        };

        public OrderTests()
        {
            mockOrder = new Mock<Order>();
        }

        public static IEnumerable<object[]> GetAllExceptMug()
        {
            yield return new object[] {ProductType.Calendar, new Random().Next(100), productWidth};
            yield return new object[] {ProductType.PhotoBook, new Random().Next(100), productWidth};
            yield return new object[] {ProductType.Canvas, new Random().Next(100), productWidth};
            yield return new object[] {ProductType.Cards, new Random().Next(100), productWidth};
        }

        public static IEnumerable<object[]> GetMug()
        {
            yield return new object[] {ProductType.Mug, new Random().Next(5, 100), productWidth};
        }

        [Theory]
        [MemberData(nameof(GetAllExceptMug))]
        public void CalculateWidthSingleReturnsTrue(ProductType product, int quantity, Dictionary<string, double> productWidth)
        {
            //Arrange
            var order = new Order
            {
                Id = Guid.Empty,
                Products = new[]
                {
                    new Product
                    {
                        ProductType = product,
                        Quantity = quantity
                    }
                }
            };
            mockOrder.Object.Id = order.Id;
            mockOrder.Object.Products = order.Products;

            //Act
            mockOrder.Object.CalculateWidth(productWidth);

            //Assert
            var width = productWidth[product.ToString()];
            Assert.Equal(quantity * width, mockOrder.Object.MinWidth);
        }
        
        [Theory]
        [MemberData(nameof(GetMug))]
        public void CalculateMugWidthReturnsFalse(ProductType product, int quantity, Dictionary<string, double> productWidth)
        {
            //Arrange
            var order = new Order
            {
                Id = Guid.Empty,
                Products = new[]
                {
                    new Product
                    {
                        ProductType = product,
                        Quantity = quantity
                    }
                }
            };
            mockOrder.Object.Id = order.Id;
            mockOrder.Object.Products = order.Products;

            //Act
            mockOrder.Object.CalculateWidth(productWidth);

            //Assert
            var width = productWidth[product.ToString()];
            Assert.NotEqual(quantity * width, mockOrder.Object.MinWidth);
        }
        [Theory]
        [MemberData(nameof(GetMug))]
        public void CalculateMugWidthReturnsTrue(ProductType product, int quantity, Dictionary<string, double> productWidth)
        {
            //Arrange
            var order = new Order
            {
                Id = Guid.Empty,
                Products = new[]
                {
                    new Product
                    {
                        ProductType = product,
                        Quantity = quantity
                    }
                }
            };
            mockOrder.Object.Id = order.Id;
            mockOrder.Object.Products = order.Products;

            //Act
            mockOrder.Object.CalculateWidth(productWidth);

            //Assert
            var width = productWidth[product.ToString()];
            Assert.Equal(((quantity / 4) + quantity % 4) * width, mockOrder.Object.MinWidth);
        }
    }
}