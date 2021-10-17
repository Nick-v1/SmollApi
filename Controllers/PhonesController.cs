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

        [HttpGet("{id}")]
        public async Task<ActionResult<Phones>> GetPhones(int id)
        {
            var phoneToGet = await _phoneRepository.Get(id);
            if (phoneToGet == null)
                return NotFound();

            return await _phoneRepository.Get(id);
        }
        [HttpPost]
        public async Task<ActionResult<Phones>> PostPhones([FromBody] Phones phone)
        {
            var newPhone = await _phoneRepository.Create(phone);
            return CreatedAtAction(nameof(GetPhones), new { phoneID = newPhone.id }, newPhone);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutPhones(int id, [FromBody] Phones phone)
        {
            if (id != phone.id) {
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
