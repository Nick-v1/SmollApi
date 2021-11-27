using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IProductRepository
    { 
        Task<Product> CreateProduct(Product product);
        Task DeleteProduct(Product product);
        Task<Product> Get(int id);
        Task<IEnumerable<Product>> Get();
        Task<Product> Update(Product product);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly EshopDBContext _context;

        public ProductRepository(EshopDBContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            product.Registration = DateTime.Now;

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Product> Update(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> Get(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> Get()
        { 
            return await _context.Products.ToListAsync();
        }

    }
}
