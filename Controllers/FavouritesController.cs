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

        public FavouritesController(IFavouriteRepository favouriteRepository, IUserRepository userRepository, IPhoneRepository phoneRepository, IMapper mapper)
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
            _mapper = mapper;
        }

        [HttpPost("/api/favorites")]         //not complete
        public async Task<ActionResult<FavouriteDto>> addToFavs([FromBody] FavouriteDto f)
        {
            var user = await _userRepository.Get(f.UserId);
            var phone = await _phoneRepository.Get(f.PhoneId);

            if (user == null) return NotFound();

            if (phone == null) return NotFound();

            var favourite = _mapper.Map<Favourite>(f);

            favourite.User = user;
            favourite.Phone = phone;

            await _favouriteRepository.addToFavourite(favourite);

            return Ok(f);
        }

        [HttpDelete("/api/favorites")]
        public async Task<ActionResult> delete(int FavId)
        {
            var favourite = await _favouriteRepository.checkFav(FavId);

            if (favourite == null)
                return NotFound();

            await _favouriteRepository.remove(favourite);

            return NoContent();
        }

        [HttpGet("/api/favorites")]
        public async Task<ActionResult<FavouriteDto>> getFavs()
        {
            var favouriteslist = await _favouriteRepository.Get();

            return Ok(favouriteslist.Select(o => _mapper.Map<FavouriteDto>(o))); 
        }

        [HttpGet("/api/favorites/{userId}")]
        public async Task<ActionResult<FavouriteDto>> getFavs(int userId)
        {
            var filteredFavourites = await _favouriteRepository.Get(userId);    // returns filtered list 
            var dto = filteredFavourites.Select(i => _mapper.Map<FavouriteDto>(i));     //maps each element of the list to dto list

            return Ok(dto);
        }
    }
}
