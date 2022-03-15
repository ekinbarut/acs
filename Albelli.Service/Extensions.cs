using System.Linq;
using Albelli.Abstraction.Infrastructure;
using Albelli.Domain.Entity;

namespace Albelli.Service
{
    public static class Extensions
    {
        public static OrderDto AsDto(this Order order)
        {
            return new (order.Id, order.Products.Select(c=> c.AsDto()).ToList(), order.MinWidth);
        }

        private static ProductDto AsDto(this Product product)
        {
            return new (product.ProductType.ToString(), product.ProductType, product.Quantity);
        }

        public static Order AsEntity(this OrderDto dto)
        {
            return new()
            {
                Id = dto.Id,
                Products = dto.ProductDtos.Select(c => c.AsEntity()),
                MinWidth = dto.MinWidth.Value
            };
        }
        
        public static Product AsEntity(this ProductDto dto)
        {
            return new()
            {
                Quantity = dto.Quantity,
                ProductType = dto.Product
            };
        }
    }
}
