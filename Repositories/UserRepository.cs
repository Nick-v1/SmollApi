using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> Get();
        Task<User> Get(int phoneID);
        Task<User> Create(User user);
        Task Update(User user);
        Task Delete(User user);
        Task Verify(int userID);
    }
    public class UserRepository : IUserRepository
    {
        private readonly EshopDBContext _context;

        public UserRepository(EshopDBContext context)
        {
            _context = context;
        }

        public async Task<User> Create(User user)
        {
            user.AccounType = "Basic";
            user.Verified = 0;
            user.Status = "Active";
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Get(int phoneID)
        {
            return await _context.Users.FindAsync(phoneID);
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            
            await _context.SaveChangesAsync();
        }

        public async Task Verify(int userID)
        {
            var userToVer = await _context.Users.FindAsync(userID);
            userToVer.Verified = 1;
            await _context.SaveChangesAsync();
        }
    }
}
