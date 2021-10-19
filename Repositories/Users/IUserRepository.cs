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
        Task<User> Get(int userid);
        Task<User> Create(User user);
        Task Update(User user);
        Task Delete(int userid);
        Task Verify(int userid);
    }
}
