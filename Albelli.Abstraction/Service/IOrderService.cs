using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Albelli.Abstraction.Infrastructure;

namespace Albelli.Abstraction.Service
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrder(CreateOrderDto order);
        Task<OrderDto> UpdateOrder(Guid orderId, IEnumerable<ProductDto> products);
    }
}