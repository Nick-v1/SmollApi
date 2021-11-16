using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{

    //public interface IPhoneRepository            moved to its own cs
    //{
    //    Task<IEnumerable<Phone>> Get();
    //    Task<Phone> Get(int phoneID);
    //    Task<Phone> Create(Phone phone);
    //    Task Update(Phone phone);
    //    Task Delete(int phoneID);
    //}
    public class PhoneRepository : IPhoneRepository
    {
        private readonly EshopDBContext _context;

        public PhoneRepository(EshopDBContext context)
        {   
            _context = context;
        }
        public async Task<Phone> Create(Phone phone)
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

        public async Task<IEnumerable<Phone>> Get()
        {
            return await _context.Phones.ToListAsync();
        }

        public async Task<Phone> Get(int phoneID)
        {
            return await _context.Phones.FindAsync(phoneID);
        }

        public async Task Update(Phone phone)
        {
            _context.Phones.Update(phone);
            await _context.SaveChangesAsync();
        }
    }

}