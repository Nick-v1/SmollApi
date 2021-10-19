using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product> Create(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task Delete(int productID)
        {
            var productToDelete = await _context.Products.FindAsync(productID);
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> Get() 
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> Get(int productID)
        {
            return await _context.Products.FindAsync(productID);
        }

        public async Task Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
