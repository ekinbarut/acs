using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albelli.Abstraction.Infrastructure;
using Albelli.Abstraction.Service;
using Albelli.Domain.Entity;
using Consul;
using Newtonsoft.Json;

namespace Albelli.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IConsulClient _consulClient;
        private readonly Dictionary<string, double> productWidth;

        public OrderService(IConsulClient consulClient)
        {
            this._consulClient = consulClient;
            var pW = _consulClient.KV.Get("ProductWidth");
            var pWString = System.Text.Encoding.UTF8.GetString(pW.Result.Response.Value);
            this.productWidth = JsonConvert.DeserializeObject<Dictionary<string, double>>(pWString);
        }

        public async Task<OrderDto> CreateOrder(CreateOrderDto orderDto)
        {
            var order = new Order()
            {
                Products = orderDto.Products.Select(c => c.AsEntity()).AsEnumerable(),
            };
            order.CalculateWidth(productWidth);

            return order.AsDto();
        }

        public async Task<OrderDto> UpdateOrder(Guid orderId, IEnumerable<ProductDto> products)
        {
            var order = new Order()
            {
                Id = orderId,
                Products = products.Select(c => c.AsEntity()).AsEnumerable(),
            };
            order.CalculateWidth(productWidth);

            return order.AsDto();
        }
    }
}