using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IPhoneRepository
    {
        Task<IEnumerable<Phone>> Get();
        Task<Phone> Get(int phoneID);
        Task<Phone> Create(Phone phone);
        Task Update(Phone phone);
        Task Delete(int phoneID);
    }
}
