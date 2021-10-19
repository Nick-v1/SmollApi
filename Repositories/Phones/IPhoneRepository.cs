using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IPhoneRepository
    {
        Task<IEnumerable<Phones>> Get();

        Task<Phones> Get(int phoneID);
        Task<Phones> Create(Phones phone);
        Task Update(Phones phone);
        Task Delete(int phoneID);
    }
}
