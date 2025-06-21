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
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao atualizar contato.", details = ex.Message });
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            try
            {
                await _contactService.DeleteByClientIdAsync(clientId);
                return Ok(new SuccessResponseDTO { Success = true, Message = "Contato removido com sucesso.", Id = clientId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao remover contato.", details = ex.Message });
            }
        }
    }
}