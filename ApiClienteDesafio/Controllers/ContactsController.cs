using Microsoft.AspNetCore.Mvc;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;
using ApiClienteDesafio.Services;
using ApiClienteDesafio.Validators;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly IMapper _mapper;

        public ContactsController(ContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var contacts = await _contactService.GetByClientIdAsync(clientId);
            var contactsDto = _mapper.Map<List<ContactDTO>>(contacts);
            return Ok(contactsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactService.GetByIdAsync(id);
            if (contact == null) return NotFound();
            var contactDto = _mapper.Map<ContactDTO>(contact);
            return Ok(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactDTO contactDto)
        {
            var contact = _mapper.Map<ContactModel>(contactDto);
            if (!ContactValidator.IsValid(contact, out var error))
                return BadRequest(error);

            var created = await _contactService.AddAsync(contact);
            var createdDto = _mapper.Map<ContactDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContactDTO contactDto)
        {
            var contact = _mapper.Map<ContactModel>(contactDto);
            contact.Id = id;
            if (!ContactValidator.IsValid(contact, out var error))
                return BadRequest(error);

            await _contactService.UpdateAsync(contact);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contactService.DeleteAsync(id);
            return NoContent();
        }
    }
}