using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Albelli.Abstraction.Entity;
using Albelli.Abstraction.Enum;
using Albelli.Abstraction.Infrastructure;
using Albelli.Abstraction.Service;
using Albelli.Domain.Entity;
using Albelli.Service;
using Consul;

namespace Albelli.API.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _repository;
        private readonly IConsulClient _consulClient;
        private readonly IOrderService _orderService;
        public OrderController(IRepository<Order> repository, IConsulClient consulClient, IOrderService orderService)
        {
            this._repository = repository;
            this._consulClient = consulClient;
            this._orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var items = (await _repository.GetAllAsync()).Select(a => a.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(Guid id)
        {
            var item = await _repository.GetAsync(id);
            if (item == null)
            {
                NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(CreateOrderDto createOrderDto)
        {
           
            var orderDto = await _orderService.CreateOrder(createOrderDto);

            var order = orderDto.AsEntity();
            
            await _repository.CreateAsync(order);

            // ReSharper disable once Mvc.ActionNotResolved
            return Ok(orderDto);
        }

        [HttpPut]
        public async Task<ActionResult<OrderDto>> PutAsync(UpdateOrderDto updateItemDto)
        {
            var existingOrder = await _repository.GetAsync(updateItemDto.Id);
        
            if (existingOrder == null)
            {
                return NotFound();
            }

            var orderDto = await _orderService.UpdateOrder(updateItemDto.Id, updateItemDto.ProductDtos);

            var order = orderDto.AsEntity();
            
            await _repository.UpdateAsync(order);
        
            return Ok(orderDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _repository.GetAsync(id);

            if (item == null)
            {
                return NotFound();
            }
            await _repository.RemoveAsync(item.Id);

            return NoContent();
        }
    }
}
