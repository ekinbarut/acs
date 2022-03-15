using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Albelli.Abstraction.Enum;

namespace Albelli.Abstraction.Infrastructure
{
    public record ProductDto(string ProductName, ProductType Product, int Quantity);
    public record OrderDto(Guid Id, IEnumerable<ProductDto> ProductDtos, double? MinWidth);
    public record UpdateOrderDto(Guid Id, IEnumerable<ProductDto> ProductDtos, double? MinWidth);
    public record CreateOrderDto([Required] IEnumerable<ProductDto> Products);
}
