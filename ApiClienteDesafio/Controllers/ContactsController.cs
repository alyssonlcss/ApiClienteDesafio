using Microsoft.AspNetCore.Mvc;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;
using ApiClienteDesafio.Services;
using AutoMapper;
using System.Threading.Tasks;
using ApiClienteDesafio.Utils;
using System.ComponentModel.DataAnnotations;

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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ContactUpdateDTO contactUpdate)
        {
            try
            {
                var success = await _contactService.UpdateByClientIdAsync(contactUpdate);
                if (!success)
                    return BadRequest(new { error = "Contact not found for this client or already exists another contact." });
                var updated = await _contactService.GetByClientIdAsync(contactUpdate.ClientId);
                var updatedDto = _mapper.Map<ContactDTO>(updated);
                return Ok(updatedDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error while updating contact.", details = ex.Message });
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            await _contactService.DeleteByClientIdAsync(clientId);
            return Ok(new SuccessResponseDTO { Success = true, Message = "Contact successfully deleted.", Id = clientId });
        }
    }
}