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

        public ProductsController(IProductRepository productRepository, IPhoneRepository phoneRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _phoneRepository = phoneRepository;
            _mapper = mapper;
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
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductCREATEDto productDto, int PhoneId)
        {
            var phone = await _phoneRepository.Get(PhoneId);

            if (phone == null) return NotFound();

            var product = _mapper.Map<Product>(productDto);
           
            product.Phone = phone;
            product.PhoneId = PhoneId;

            var newProduct = await _productRepository.CreateProduct(product);
            return CreatedAtAction(nameof(GetProducts), newProduct);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Product>> PatchProduct(int id, [FromBody] ProductCREATEDto productDto)
        {
            var productCheck = await _productRepository.Get(id);

            if (productCheck == null)
                return NotFound();

            var product = _mapper.Map<Product>(productCheck);
            product.Phone = productCheck.Phone;
            product.Price = productDto.price;

            return Ok(await _productRepository.Update(product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productToDelete = await _productRepository.Get(id);

            if (productToDelete == null)
                return NotFound();

            await _productRepository.DeleteProduct(productToDelete);

            return NoContent();
        }
    }
}