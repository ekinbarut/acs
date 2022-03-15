using System;
using System.Collections.Generic;
using Albelli.Abstraction.Entity;
using Albelli.Abstraction.Enum;

namespace Albelli.Domain.Entity
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public double MinWidth { get; set; }

        public void CalculateWidth(Dictionary<string, double> productWidth)
        {
            foreach (var product in Products)
            {
                MinWidth += product.ProductType switch
                {
                    ProductType.Calendar => product.Quantity * productWidth[product.ProductType.ToString()],
                    ProductType.Canvas => product.Quantity * productWidth[product.ProductType.ToString()],
                    ProductType.Cards => product.Quantity * productWidth[product.ProductType.ToString()],
                    ProductType.PhotoBook => product.Quantity * productWidth[product.ProductType.ToString()],
                    ProductType.Mug => (product.Quantity / 4 + (product.Quantity % 4)) * productWidth[product.ProductType.ToString()],
                    _ => MinWidth
                };
            }
        }
    }
}
