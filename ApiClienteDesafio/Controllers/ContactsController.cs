using Microsoft.AspNetCore.Mvc;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;
using ApiClienteDesafio.Services;
using AutoMapper;
using System.Threading.Tasks;
using ApiClienteDesafio.Utils;

namespace ApiClienteDesafio.Controllers
{
    [ApiController]
    [Route("contacts")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly IMapper _mapper;

        public ContactsController(ContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var contact = await _contactService.GetByClientIdAsync(clientId);
            if (contact == null) return NotFound();
            var contactDto = _mapper.Map<ContactDTO>(contact);
            return Ok(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactCreateDTO contactDTO)
        {
            var contact = _mapper.Map<ContactModel>(contactDTO);
            if (!ValidationUtils.TryValidateObject(contact, out var validationResults))
                return BadRequest(string.Join("; ", validationResults.Select(v => v.ErrorMessage)));
            var created = await _contactService.AddAsync(contact);
            if (created == null) return BadRequest("A client can only have one contact or ClientId does not exist.");
            var createdDto = _mapper.Map<ContactDTO>(created);
            return CreatedAtAction(nameof(GetByClientId), new { clientId = contact.ClientId }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ContactCreateDTO contactDTO)
        {
            var contact = _mapper.Map<ContactModel>(contactDTO);
            if (!ValidationUtils.TryValidateObject(contact, out var validationResults))
                return BadRequest(string.Join("; ", validationResults.Select(v => v.ErrorMessage)));
            var success = await _contactService.UpdateByClientIdAsync(contact);
            if (!success) return BadRequest("Contact not found for this client or already exists another contact.");
            return NoContent();
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            await _contactService.DeleteByClientIdAsync(clientId);
            return NoContent();
        }
    }
}