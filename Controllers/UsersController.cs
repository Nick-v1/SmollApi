/*using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.Get();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var userToGet = await _userRepository.Get(id);
            if (userToGet == null)
                return NotFound();

            return userToGet;
        }

        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
        {
            var newUser = await _userRepository.Create(user);
            return CreatedAtAction(nameof(GetUsers), new { newUser.id }, newUser);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            user.id = id;
            if (await _userRepository.Get(user.id) == null)
                return NotFound();

            await _userRepository.Update(user);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userToDelete = await _userRepository.Get(id);
            if (userToDelete == null)
                return NotFound();

            await _userRepository.Delete(id);
            return NoContent();
        }
        [HttpPatch("/api/Users/verify/{id}")]
        public async Task<ActionResult> Verify(int id)
        {
            await _userRepository.Verify(id);
            return NoContent();
        }
    }
}
*/