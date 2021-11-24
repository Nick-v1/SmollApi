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
    [Route("api/[controller]")]
    [ApiController]
    public class BansController : ControllerBase
    {
        private readonly IBanRepository _banRepository;
        private readonly IUserRepository _userRepository;

        public BansController(IBanRepository banRepository, IUserRepository userRepository)
        {
            _banRepository = banRepository;
            _userRepository = userRepository;
        }

        [HttpPost("User")]
        public async Task<ActionResult<Ban>> Ban([FromBody] Ban ban)
        {
            try
            {
                var userToBan = await _userRepository.Get(ban.UserId);
                if (userToBan == null)
                    return NotFound("User not found");

                await _banRepository.BanSomeone(ban);

                return CreatedAtAction(nameof(Ban), ban);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status302Found,
                $"This user is already banned");
            }
        }

        [HttpGet]
        public async Task<ActionResult<Ban>> List()
        {
            var banlist = await _banRepository.Get();

            return Ok(banlist);
        }
        [HttpGet("/CheckBan/{id}")]
        public async Task<ActionResult<Ban>> checkBan(int id) 
        {
            var user = await _userRepository.Get(id);

            if (user == null)
                return NotFound("User does not exist");
            
            var ban = await _banRepository.checkBan(id);

            if (ban == null)
                return Ok("User has no ban history");

            return Ok($"User has ban logs\nThey were banned at: {ban.BannedDate}\nReason: {ban.Reason}");
        }

        [HttpDelete("unban/{id}")]
        public async Task<ActionResult<Ban>> unBan(int id)
        {
            try
            {
                var user = await _userRepository.Get(id);

                if (user == null)
                    return NotFound("User does not exist");

                await _banRepository.RevokeBan(id);

                return NoContent();
            } catch(Exception)
            {
                return StatusCode(StatusCodes.Status302Found, "This user is not banned");
            }
        }

    }
}
