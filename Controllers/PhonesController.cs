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
    public class PhonesController : ControllerBase
    {
        private readonly IPhoneRepository _phoneRepository;
        public PhonesController(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<Phones>> GetPhones()
        {
            return await _phoneRepository.Get();
        }

        [HttpGet("{phoneID}")]
        public async Task<ActionResult<Phones>> GetPhones(int phoneID)
        {
            return await _phoneRepository.Get(phoneID);
        }
        [HttpPost]
        public async Task<ActionResult<Phones>> PostPhones([FromBody] Phones phone)
        {
            var newPhone = await _phoneRepository.Create(phone);
            return CreatedAtAction(nameof(GetPhones), new { phoneID = newPhone.id }, newPhone);
        }

        [HttpPut]
        public async Task<ActionResult> PutPhones(int phoneID, [FromBody] Phones phone)
        {
            if (phoneID != phone.id) {
                return BadRequest();
            }
            await _phoneRepository.Update(phone);

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var phoneToDelete = await _phoneRepository.Get(id);
            if (phoneToDelete == null)
                return NotFound();

            await _phoneRepository.Delete(phoneToDelete.id);
            return NoContent();
        }
    }
}
