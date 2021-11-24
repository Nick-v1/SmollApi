using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmollApi.Models.Dtos;

namespace SmollApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSearchDto>>> GetUsers()
        {
            return Ok((await _userRepository.Get()).Select(o => _mapper.Map<UserSearchDto>(o)));
        }
        [HttpGet("{id}")]           // for url path {}
        public async Task<ActionResult<UserSearchDto>> GetUser(int id)
        {
            var userToGet = await _userRepository.Get(id);
            if (userToGet == null)
                return NotFound();

            return Ok(_mapper.Map<UserSearchDto>(userToGet));
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<UserAccountManagement>> RegisterUser([FromBody] UserAccountManagement userDto)
        {
            var user = _mapper.Map<User>(userDto);         // transforms userDto to User; //Also it needs this line: CreateMap<UserCreateAccountDto, User>(); on AutoMapperProfile
            //var newUser = await _userRepository.Create(user);
            //return CreatedAtAction(nameof(GetUsers), new { newUser.Id }, newUser);
            await _userRepository.Create(user);
            return Created($"/api/users/{user.Id}", _mapper.Map<UserAccountManagement>(user));
        }
        [HttpPost("/api/Users/Login")]
        public async Task Login([FromBody] UserLoginDto userDto)
        { 

            //login method. to be implemented later;
        }
        [HttpPut("{id}")]          //user update
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserAccountManagement userDto)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                return NotFound("User Not Found");

            //user.Username = userDto.Username;
            //user.Email = userDto.Email;           //or just use mapper;
            //user.Password = userDto.Password;

            _mapper.Map(userDto, user);            // <--
            
            await _userRepository.Update(user);

            return Ok($"User with id: {id} has been updated");
        }
        [HttpPut("{id}/Admin")]   //admin update
        public async Task<ActionResult> UpdateUserAdmin(int id, [FromBody] UserDtoDetails userDto)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
                return NotFound("User Not Found");

            //user.Username = userDto.Username;
            //user.Email = userDto.Email;           //or just use mapper;
            //user.Password = userDto.Password;

            _mapper.Map(userDto, user);            // <--

            await _userRepository.Update(user);

            return Ok($"User with id: {id} has been updated");
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
                var UserToVerify = await _userRepository.Get(id);

                if (UserToVerify == null)
                    return NotFound($"User with id: {id} not found");

                await _userRepository.Verify(id);

                return Ok("User has been verified");
        }
    }
}