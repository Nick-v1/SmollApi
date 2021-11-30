using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmollApi.Models;
using SmollApi.Models.Dtos;
using SmollApi.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonesController : ControllerBase
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public PhonesController(IPhoneRepository phoneRepository, IMapper mapper, ITokenService tokenService)
        {
            _phoneRepository = phoneRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phone>>> GetPhones()
        {
            var phone = await _phoneRepository.Get();                                      //in 2 steps
            //var phonedto = phone.Select(o => _mapper.Map<PhoneDto>(o));

            //var phoneDto = (await _phoneRepository.Get()).Select(o => _mapper.Map<PhoneDto>(o));    //in 1 step

            //return Ok((await _phoneRepository.Get()).Select(o => _mapper.Map<PhoneDto>(o))); // instant return in 1 line
            return Ok(phone);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Phone>> GetPhones(int id)
        {
            var phoneToGet = await _phoneRepository.Get(id);
            if (phoneToGet == null)
                return NotFound();

            return Ok(await _phoneRepository.Get(id));
        }
        [HttpPost]
        public async Task<ActionResult<Phone>> PostPhones([FromBody] PhoneDto phonedto, [FromHeader]string token)
        {
            if (token != null)
            {
                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleclaim = _tokenService.GetClaim(token, "UserRole");
                    if (roleclaim.Equals("Merchant") || roleclaim.Equals("Admin"))
                    {
                        var phone = _mapper.Map<Phone>(phonedto);
                        var newPhone = await _phoneRepository.Create(phone);
                        return CreatedAtAction(nameof(GetPhones), newPhone);
                    }
                    return Unauthorized();
                }
            }

            return Unauthorized();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPhones(int id, [FromBody] PhoneDto phoneDto, [FromHeader] string token)
        {
            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleclaim = _tokenService.GetClaim(token, "UserRole");
                    if (roleclaim.Equals("Admin"))
                    {
                        var phoneToChange = await _phoneRepository.Get(id);
                        if (phoneToChange == null) return NotFound();

                        //phoneToChange.Manifacturer = phone.Manifacturer;
                        //phoneToChange.Name = phone.Name;                  //can implement mapper 
                        //phoneToChange.OS = phone.OS;
                        //phoneToChange.RAM = phone.RAM;
                        //phoneToChange.ROM = phone.ROM;
                        //phoneToChange.ScreenSize = phone.ScreenSize;

                        _mapper.Map(phoneDto, phoneToChange);

                        await _phoneRepository.Update(phoneToChange);

                        return Ok(phoneToChange);
                    }
                    else return Unauthorized();
                }
            }
            return Unauthorized();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromHeader] string token)
        {
            if (token != null)
            {

                var isValid = _tokenService.ValidateCurrentToken(token);

                if (isValid)
                {
                    var roleclaim = _tokenService.GetClaim(token, "UserRole");
                    if (roleclaim.Equals("Admin"))
                    {
                        var phoneToDelete = await _phoneRepository.Get(id);
                        if (phoneToDelete == null)
                            return NotFound();

                        await _phoneRepository.Delete(phoneToDelete.Id);
                        return NoContent();
                    }
                    else return Unauthorized();
                }
            }
            return Unauthorized();
        }
    }
}
