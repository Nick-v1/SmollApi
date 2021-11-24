using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
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

        public FavouritesController(IFavouriteRepository favouriteRepository, IUserRepository userRepository, IPhoneRepository phoneRepository)
        {
            _favouriteRepository = favouriteRepository;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
        }

        [HttpPost("/User/{UserID}/Favorites/{PhoneID}")]         //not complete
        public async Task<ActionResult<Favourite>> addToFavs(int UserID, int PhoneID)
        {
            var Phone = await _phoneRepository.Get(PhoneID);
            var User = await _userRepository.Get(UserID);

            if (User == null)
                return NotFound("User not found");
            
            if (Phone == null)
                return NotFound("Phone not found");

            var fav = new Favourite();

            fav.UserId = UserID;
            fav.PhoneId = PhoneID;

            await _favouriteRepository.addToFavourite(new Favourite(), UserID, PhoneID);

            return Ok();
        }
        [HttpDelete("/User/{UserID}/Favorites/{PhoneID}")]
        public async Task<ActionResult> delete(int UserID, int PhoneID)
        {
            var Phone = await _phoneRepository.Get(PhoneID);
            var User = await _userRepository.Get(UserID);

            if (User == null)
                return NotFound("User not found");

            if (Phone == null)
                return NotFound("Phone not found");

            var fav = new Favourite();
            fav.PhoneId = PhoneID;
            fav.UserId = UserID;

            await _favouriteRepository.remove(fav);

            return NoContent();
        }
    }
}
