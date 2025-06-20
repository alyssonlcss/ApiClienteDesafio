using Microsoft.AspNetCore.Mvc;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;
using ApiClienteDesafio.Services;
using ApiClienteDesafio.Validators;
using AutoMapper;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Controllers
{
    [ApiController]
    [Route("api/clients/{clientId}/contact")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly IMapper _mapper;

        public ContactsController(ContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var contact = await _contactService.GetByClientIdAsync(clientId);
            if (contact == null) return NotFound();
            var contactDto = _mapper.Map<ContactDTO>(contact);
            return Ok(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int clientId, [FromBody] ContactCreateDTO contactDto)
        {
            var contact = _mapper.Map<ContactModel>(contactDto);
            var created = await _contactService.AddAsync(clientId, contact);
            if (created == null) return BadRequest("A client can only have one contact or ClientId does not exist.");
            var createdDto = _mapper.Map<ContactDTO>(created);
            return CreatedAtAction(nameof(GetByClientId), new { clientId }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int clientId, [FromBody] ContactDTO contactDto)
        {
            var contact = _mapper.Map<ContactModel>(contactDto);
            var success = await _contactService.UpdateByClientIdAsync(clientId, contact);
            if (!success) return BadRequest("Contact not found for this client or already exists another contact.");
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int clientId)
        {
            await _contactService.DeleteByClientIdAsync(clientId);
            return NoContent();
        }
    }
}