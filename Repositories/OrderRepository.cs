using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> Create(Order order);
        Task Delete(Order order);
        Task<Order> Get(int id);
        Task<IEnumerable<Order>> Get();
        Task Update(Order order);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly EshopDBContext _context;

        public OrderRepository(EshopDBContext context)
        {
            _context = context;
        }
        public async Task<Order> Create(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> Get(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> Get()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task Update(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}