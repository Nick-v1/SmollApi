using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Models.Dtos;
using SmollApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto login)
        {
            IActionResult response = NotFound();
            var user = await AuthenticateUser(login);

            if (user != null)
            {
                var generatedToken = _tokenService.GenerateToken(user);
                return Ok(new { token = generatedToken, status = "Success" });
            }

            return response;
        }

        /// <summary>
        /// Authenticate method is used by login
        /// </summary>
        /// <param name="login">user model</param>
        /// <returns>A user if exists, null if not</returns>
        private async Task<User> AuthenticateUser(UserLoginDto login)
        {
            var user = await _userRepository.GetUserByEmail(login.Email);

            if (user == null) return null;

            if (login.Email == user.Email && login.Password == user.Password)
            {
                return user;
            }

            return null;
        }
    }
}
