using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Models.Dtos;
using SmollApi.Repositories;
using System;
using System.Collections.Generic;
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
        private readonly ITokenService _tokenService;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, IMapper mapper, ITokenService tokenService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders([FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var role = _tokenService.GetClaim(token, "UserRole");
                    if (role.Equals("Merchant") || role.Equals("Admin"))
                        return Ok(await _orderRepository.Get());

                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var role = _tokenService.GetClaim(token, "UserRole");
                    if (role.Equals("Merchant") || role.Equals("Admin"))
                    {
                        var s = await _orderRepository.Get(id);
                        if (s == null) return NotFound();

                        return Ok(s);
                    }
                    return Unauthorized();
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderdto, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var product = await _productRepository.Get(orderdto.ProductId);
                    var user = await _userRepository.Get(orderdto.UserId);
                    var email = _tokenService.GetClaim(token, "email");
                    var claimUser = await _userRepository.GetUserByEmail(email);

                    if (user == claimUser)
                    {
                        if (product == null) return NotFound();

                        if (user == null) return NotFound();

                        var order = _mapper.Map<Order>(orderdto);

                        order.Product = product;
                        order.User = user;
                        order.OrderDate = DateTime.Now;

                        var orderc = await _orderRepository.Create(order);

                        return CreatedAtAction(nameof(CreateOrder), orderc);
                    }
                    return Unauthorized("You can create orders for other users");
                }
            }

            return Unauthorized("Log in to make an order");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id, [FromHeader] string token)
        {
            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var role = _tokenService.GetClaim(token, "UserRole");
                    if (role.Equals("Admin") || role.Equals("Merchant"))
                    {
                        var order = await _orderRepository.Get(id);

                        if (order == null)
                            return NotFound();

                        await _orderRepository.Delete(order);
                        return NoContent();
                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

    }
}
