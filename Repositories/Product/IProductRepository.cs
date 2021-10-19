using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> Get();

        Task<Product> Get(int productID);
        Task<Product> Create(Product product);
        Task Update(Product product);
        Task Delete(int productID);
    }
}
