using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmollApi.Models;
using SmollApi.Models.Dtos;
using SmollApi.Repositories;
using System.Threading.Tasks;

namespace SmollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BansController : ControllerBase
    {
        private readonly IBanRepository _banRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public BansController(IBanRepository banRepository, IUserRepository userRepository, IMapper mapper)
        {
            _banRepository = banRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("{UserId}")]
        public async Task<ActionResult<BanDtoResult>> Ban(int UserId, [FromBody] BanDto banDto)
        {
            
            var userToBan = await _userRepository.Get(UserId);       // gets the user by id
            if (userToBan == null)
                return NotFound("User not found");                  //return not found if doesn't exist

            if (userToBan.Status.Equals("Banned"))            //return msg if he's already banned
                return Ok("User is already banned");

            var ban = _mapper.Map<Ban>(banDto);              //map the dto FromBody to Ban

            ban.User = userToBan;                           //set its values
            ban.User.Status = "Banned";

            await _banRepository.BanSomeone(ban);           //send it to repo for creation

            return CreatedAtAction(nameof(Ban), _mapper.Map<BanDtoResult>(ban)); //return  response
        }

        [HttpPost("/api/unban")]
        public async Task<ActionResult<BanDtoResult>> unBan([FromBody] UnbanDto Unbandto)
        {
            var user = await _userRepository.Get(Unbandto.UserId);     //gets the user by id

            if (user == null)
                return NotFound("User not found");             //checks if user exists


            if (!user.Status.Equals("Banned"))                //checks if they are already unbanned
                return Ok("User is not banned");

            var ban = _mapper.Map<Ban>(Unbandto);            //map UnbanDto to Ban
            ban.User = user;                                 //set Ban's User
            ban.User.Status = "Unbanned";                    //change his status                       
                
            var banresult = await _banRepository.RevokeBan(ban);   //calls repo

            return CreatedAtAction(nameof(unBan), _mapper.Map<BanDtoResult>(banresult)); //return result
        }

        [HttpGet]
        public async Task<ActionResult<Ban>> List()
        {
            var banlist = await _banRepository.Get();

            return Ok(banlist);
        }
        [HttpGet("/api/checkBan/{BanId}")]
        public async Task<ActionResult<Ban>> checkBan(int BanId) 
        {
            var ban = await _banRepository.checkBan(BanId);

            if (ban == null)
                return NotFound();

            return Ok(ban);
        }
    }
}
