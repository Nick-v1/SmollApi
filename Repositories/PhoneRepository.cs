using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly PhoneContext _context;

        public PhoneRepository(PhoneContext context)
        {
            _context = context;
        }
        public async Task<Phones> Create(Phones phone)
        {
            _context.Phones.Add(phone);
            await _context.SaveChangesAsync();

            return phone;
        }

        public async Task Delete(int phoneID)
        {
            var phoneToDelete = await _context.Phones.FindAsync(phoneID);
            _context.Phones.Remove(phoneToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Phones>> Get()
        {
            return await _context.Phones.ToListAsync(); 
        }

        public async Task<Phones> Get(int phoneID)
        {
            return await _context.Phones.FindAsync(phoneID);
        }

        public async Task Update(Phones phone)
        {
            _context.Entry(phone).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
