using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }
        public async Task<User> Create(User user)
        {
            _context.users.Add(user);
            user.setType("Basic");
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task Delete(int userid)
        {
            var userToDelete = await _context.users.FindAsync(userid);
            _context.users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> Get()
        {
            var users = await _context.users.ToListAsync(); 

            if (users.Count() > 10)
            {
                users.Take(10).ToList();
                var paginationUsers = users.Take(10).ToList();
                return paginationUsers;
            }

            return users;
        }

        public async Task<User> Get(int userid)
        {
            return await _context.users.FindAsync(userid);
        }

        public async Task Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Verify(int userid)
        {
            var userToVerify = await _context.users.FindAsync(userid);

            //if (userToVerify.accounType.Equals("Basic"))
            //{ return StatusCodes.Status405MethodNotAllowed; }
            userToVerify.setVer(true);

            _context.Entry(userToVerify).State = EntityState.Modified;
            //private set//userToVerify.verified = true;
            await _context.SaveChangesAsync();
        }
    }
}
