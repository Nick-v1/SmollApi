using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Models.Dtos;
using SmollApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return Ok(await _orderRepository.Get());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var s = await _orderRepository.Get(id);
            if (s == null) return NotFound();

            return Ok(s);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderdto)
        {

            var product = await _productRepository.Get(orderdto.ProductId);
            var user = await _userRepository.Get(orderdto.UserId);

            if (product == null) return NotFound();

            if (user == null) return NotFound();

            var order = _mapper.Map<Order>(orderdto);

            order.Product = product;
            order.User = user;
            order.OrderDate = DateTime.Now;

            var orderc = await _orderRepository.Create(order);

            return CreatedAtAction(nameof(CreateOrder), orderc);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.Get(id);

            if (order == null)
                return NotFound();

            await _orderRepository.Delete(order);
            return NoContent();
        }

    }
}
