using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IBanRepository
    {
        Task<IEnumerable<Ban>> Get(); //get the ban list
        Task<Ban> checkBan(int userID);   //get a specific banned user
        Task<Ban> BanSomeone(Ban ban); //bans a user
        Task RevokeBan(int phoneID);
    }
    public class BanRepository : IBanRepository
    {
        private readonly EshopDBContext _context;

        public BanRepository(EshopDBContext context)
        {
            _context = context;
        }

        public async Task<Ban> BanSomeone(Ban ban)
        {
            ban.setDate(DateTime.Now);
            _context.Bans.Add(ban);
            
            await _context.SaveChangesAsync();

            return ban;
        }

        public async Task<IEnumerable<Ban>> Get()
        {
            return await _context.Bans.ToListAsync();
        }

        public async Task<Ban> checkBan(int userID)
        {
            return await _context.Bans.FindAsync(userID);
        }

        public async Task RevokeBan(int userID)
        {
            var ban = await _context.Bans.FindAsync(userID);

            _context.Bans.Remove(ban);
            await _context.SaveChangesAsync();
        }
    }
}
