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
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public ProductsController(IProductRepository productRepository, IPhoneRepository phoneRepository, IMapper mapper, ITokenService tokenService)
        {
            _productRepository = productRepository;
            _phoneRepository = phoneRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productRepository.Get();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var productToGet = await _productRepository.Get(id);
            if (productToGet == null)
                return NotFound();

            return productToGet;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCREATEDto productDto, int PhoneId, [FromHeader] string token)
        {
            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleclaim = _tokenService.GetClaim(token, "UserRole");

                    if (roleclaim.Equals("Admin") || roleclaim.Equals("Merchant"))
                    {
                        var phone = await _phoneRepository.Get(PhoneId);

                        if (phone == null) return NotFound();

                        var product = _mapper.Map<Product>(productDto);

                        product.Phone = phone;
                        product.PhoneId = PhoneId;

                        var newProduct = await _productRepository.CreateProduct(product);
                        return CreatedAtAction(nameof(GetProducts), newProduct);
                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Product>> PatchProduct(int id, [FromBody] ProductCREATEDto productDto, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleclaim = _tokenService.GetClaim(token, "UserRole");
                    if (roleclaim.Equals("Admin") || roleclaim.Equals("Merchant"))
                    {
                        var productCheck = await _productRepository.Get(id);

                        if (productCheck == null)
                            return NotFound();

                        var product = _mapper.Map<Product>(productCheck);
                        product.Phone = productCheck.Phone;
                        product.Price = productDto.price;

                        return Ok(await _productRepository.Update(product));
                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleclaim = _tokenService.GetClaim(token, "UserRole");

                    if (roleclaim.Equals("Admin") || roleclaim.Equals("Merchant"))
                    {
                        var productToDelete = await _productRepository.Get(id);

                        if (productToDelete == null)
                            return NotFound();

                        await _productRepository.DeleteProduct(productToDelete);

                        return NoContent();
                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }
    }
}