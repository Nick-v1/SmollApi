using AutoMapper;
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
    [Route("api/[Controller]")]
    public class FavouritesController : ControllerBase
    {
        private readonly IFavouriteRepository _favouriteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public FavouritesController(IFavouriteRepository favouriteRepository, IUserRepository userRepository, IPhoneRepository phoneRepository, IMapper mapper, ITokenService tokenService)
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("/api/favorites")]         //not complete
        public async Task<ActionResult<FavouriteDto>> addToFavs([FromBody] FavouriteDto f, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var emailClaim = _tokenService.GetClaim(token, "email");
                    var user = await _userRepository.Get(f.UserId);
                    if (emailClaim.Equals(user.Email))
                    {
                        var phone = await _phoneRepository.Get(f.PhoneId);

                        if (phone == null) return NotFound("Phone not found");

                        var favourite = _mapper.Map<Favourite>(f);

                        favourite.User = user;
                        favourite.Phone = phone;

                        var list = await _favouriteRepository.Get(user.Id);
                        var s = list.FirstOrDefault(o => o.UserId == user.Id && o.PhoneId == phone.Id);

                        if (s == null)
                        {
                            await _favouriteRepository.addToFavourite(favourite);

                            return Ok("You added an item to your favorites");
                        }

                        if (s.UserId == user.Id && s.PhoneId == phone.Id)
                            return BadRequest("This item is already in your favorites");

                    }
                    return Unauthorized("You may only add favorites under your name");
                }
            }
            return Unauthorized("Log in to add an item to your favorites");
        }

        [HttpDelete("/api/favorites")]
        public async Task<ActionResult> delete(int FavId, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var emailClaim = _tokenService.GetClaim(token, "email");
                    var user = await _userRepository.GetUserByEmail(emailClaim);

                    var favourite = await _favouriteRepository.checkFav(FavId);

                    if (favourite == null)
                        return NotFound();

                    if (user.Id == favourite.UserId)
                    {
                        await _favouriteRepository.remove(favourite);
                        return NoContent();
                    }
                    else
                        return Unauthorized("You can't remove others' items");
                }
            }
            return Unauthorized();
        }

        [HttpGet("/api/favorites/admin")]         //admin get
        public async Task<ActionResult<FavouriteDto>> getFavs([FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleClaim = _tokenService.GetClaim(token, "UserRole");
                    if (roleClaim == "Admin")
                    {
                        var favouriteslist = await _favouriteRepository.Get();

                        return Ok(favouriteslist.Select(o => _mapper.Map<FavouriteDto>(o)));
                    }
                    return Unauthorized();
                }
            }
            return Unauthorized();
        }

        [HttpGet("/api/favorites/{userId}")]  //gets someone's favourites. (Requires to be logged in)
        public async Task<ActionResult<FavouriteDto>> getFavs(int userId, [FromHeader] string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var filteredFavourites = await _favouriteRepository.Get(userId);    // returns filtered list 
                    var dto = filteredFavourites.Select(i => _mapper.Map<FavouriteDto>(i));     //maps each element of the list to dto list

                    return Ok(dto);
                }
            }
            return Unauthorized();
        }
    }
}
