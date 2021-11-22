using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userRepository.Get());
        }
        [HttpGet("{id}")]           // for url path {}
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var userToGet = await _userRepository.Get(id);
            if (userToGet == null)
                return NotFound();

            return Ok(userToGet);
        }

        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
        {
            var newUser = await _userRepository.Create(user);
            
            return CreatedAtAction(nameof(GetUsers), new { newUser.Id }, newUser);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                user.SetId(id);

                if (id != user.Id)
                    return BadRequest();

                if (await _userRepository.Get(user.Id) == null)
                    return NotFound("User not found");

                await _userRepository.Update(user);

                return Ok($"User with id: {id} has been updated");
            }
            catch (Exception e)
            { 
                return StatusCode(StatusCodes.Status500InternalServerError,
                $"Error updating data = {e.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userToDelete = await _userRepository.Get(id);
            if (userToDelete == null)
                return NotFound("User Not Found");

            await _userRepository.Delete(userToDelete);
            return NoContent();
        }
        [HttpPatch("/api/Users/verify/{id}")]
        public async Task<ActionResult> Verify(int id)
        {
            try
            {
                var UserToVerify = await _userRepository.Get(id);

                if (UserToVerify == null)
                    return NotFound($"User with id: {id} not found");

                await _userRepository.Verify(id);

                return Ok("User has been verified");
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
            }
        }
    }
}