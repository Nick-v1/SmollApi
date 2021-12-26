using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmollApi.Models.Dtos;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace SmollApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UsersController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserSearchDto>>> GetUsers([FromHeader]string token)
        {
            if (token != null)
            {
                var tokenvalid = _tokenService.ValidateCurrentToken(token);

                if (tokenvalid)
                {
                    var claimValue = _tokenService.GetClaim(token, "UserRole");

                    if (claimValue.Equals("Basic") || claimValue.Equals("Admin"))
                        return Ok((await _userRepository.Get()).Select(o => _mapper.Map<UserSearchDto>(o)));

                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        /// <summary>
        /// This method is only for admins.
        /// If a user tries it, it returns unauthorized message to him.
        /// If someone who is not logged in, tries it, it returns Forbidden.
        /// token: The token contains the user claims which can be used to verify the authenticity of the user
        /// if token is not there, it means that someone who is not logged in tried to call the method.
        /// </summary>
        /// <param name="id">the id of the user</param>
        /// <param name="token">the token of the user</param>
        /// <returns>All the information of the specified user</returns>
        [HttpGet("{id}")]           // for url path {}
        public async Task<ActionResult<UserSearchDto>> GetUser(int id, [FromHeader] string token)
        {
            if (token != null)
            {

                var tokenvalid = _tokenService.ValidateCurrentToken(token);

                if (tokenvalid)
                {
                    var claimValue = _tokenService.GetClaim(token, "UserRole");

                    if (claimValue.Equals("Admin"))
                    {
                        var userToGet = await _userRepository.Get(id);

                        if (userToGet == null)
                            return NotFound();

                        return Ok(userToGet);
                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpPost("signup")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] UserAccountManagement userDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetUserByEmail(userDto.Email);

                if (existingUser != null)
                {
                    return BadRequest("An account with this email already exists");
                }

                var user = _mapper.Map<User>(userDto);         // transforms userDto to User; //Also it needs this line: CreateMap<UserCreateAccountDto, User>(); on AutoMapperProfile
                                                               //var newUser = await _userRepository.Create(user);
                                                               //return CreatedAtAction(nameof(GetUsers), new { newUser.Id }, newUser);
                user.AccounType = "Basic";
                user.Verified = 0;
                user.Status = "Active";

                await _userRepository.Create(user);

                return Created($"/api/users/{user.Id}", _mapper.Map<UserAccountManagement>(user));
            }

            return BadRequest("Invalid Payload");
        }


        [HttpPut("{id}")]          //user update
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserAccountManagement userDto, [FromHeader] string token)
        {

            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var claim = _tokenService.GetClaim(token, JwtRegisteredClaimNames.Email);

                    var user = await _userRepository.Get(id);
                    if (claim.Equals(user.Email))
                    {
                        if (user == null)
                            return NotFound("User Not Found");

                        //user.Username = userDto.Username;
                        //user.Email = userDto.Email;           //or just use mapper;
                        //user.Password = userDto.Password;

                        _mapper.Map(userDto, user);            // <--

                        await _userRepository.Update(user);

                        return Ok($"User with id: {id} has been updated");
                    }
                    else
                        return Unauthorized("You can't update another user\nYou may only update your account");
                }
            }
            return Unauthorized();
        }

        /// <summary>
        /// Admin only method. Update a user from the database
        /// This api call is only available for admin accounts
        /// Can modify all the propeerties of an account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <param name="token"></param>
        /// <returns>An action result with a message</returns>
        [HttpPut("{id}/Admin")]   //admin update
        public async Task<ActionResult> UpdateUserAdmin(int id, [FromBody] UserDtoDetails userDto, [FromHeader]string token)
        {
            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid) //checks if the token is still valid
                {
                    var claim = _tokenService.GetClaim(token, "UserRole");

                    if (claim.Equals("Admin"))
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
                    else
                        return Unauthorized("You are not authorized to do that");
                }
            }
            //if token is not valid returns 403
            return Unauthorized();
        }


        /// <summary>
        /// Admin only methods. Delete a user from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromHeader] string token)
        {
            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var claim = _tokenService.GetClaim(token, "UserRole");
                    if (claim.Equals("Admin"))
                    {
                        var userToDelete = await _userRepository.Get(id);
                        if (userToDelete == null)
                            return NotFound("User Not Found");

                        await _userRepository.Delete(userToDelete);
                        return NoContent();
                    }
                    else
                        return Unauthorized();
                }
            }
            return Unauthorized();
        }

        /// <summary>
        /// Admin only methods. Verify a user from the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPatch("/api/Users/verify/{id}")]
        public async Task<ActionResult> Verify(int id, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var claim = _tokenService.GetClaim(token, "UserRole");
                    if (claim.Equals("Admin"))
                    {
                        var UserToVerify = await _userRepository.Get(id);

                        if (UserToVerify == null)
                        {
                            return NotFound($"User with id: {id} not found");
                        }

                        await _userRepository.Verify(id);

                        return Ok("Successfully verified");
                    }
                    else
                        return Unauthorized("Contact an admin in order to get verified");
                }
            }
            return Unauthorized();
        }

    }
}