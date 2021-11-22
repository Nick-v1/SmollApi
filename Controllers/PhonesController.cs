using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmollApi.Models;
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
        public PhonesController(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phone>>> GetPhones()
        {
            return Ok(await _phoneRepository.Get());
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
        public async Task<ActionResult<Phone>> PostPhones([FromBody] Phone phone)
        {
            var newPhone = await _phoneRepository.Create(phone);
            return CreatedAtAction(nameof(GetPhones), new { phoneID = newPhone.Id }, newPhone);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPhones(int id, [FromBody] Phone phone)
        {
            phone.SetId(id);

            if (await _phoneRepository.Get(phone.Id) == null) // if phone doesn't exist return not found
                return NotFound();
            
            await _phoneRepository.Update(phone);

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var phoneToDelete = await _phoneRepository.Get(id);
            if (phoneToDelete == null)
                return NotFound();

            await _phoneRepository.Delete(phoneToDelete.Id);
            return NoContent();
        }
    }
}
