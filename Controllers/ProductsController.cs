using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
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

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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

            return await _productRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            var newProduct = await _productRepository.Create(product);
            return CreatedAtAction(nameof(GetProducts), new { productID = newProduct.id, registration = DateTime.Now }, newProduct);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutProduct(int id, [FromBody] Product product)
        {
            product.id = id;
            if (await _productRepository.Get(product.id) == null)
                return NotFound();

            await _productRepository.Update(product);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var productToDelete = await _productRepository.Get(id);
            if (productToDelete == null)
                return NotFound();

            await _productRepository.Delete(productToDelete.id);
            return NoContent();
        }
    }
}
