using Microsoft.AspNetCore.Mvc;
using ApiClienteDesafio.Models;
using ApiClienteDesafio.DTOs;
using ApiClienteDesafio.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.Controllers
{
    [ApiController]
    [Route("clients")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly IMapper _mapper;

        public ClientsController(ClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet("all-clients")]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            var clientsDto = clients.Select(client => new
            {
                client.ClientId,
                client.Name,
                client.CreateDate,
                Address = client.Address != null ? _mapper.Map<AddressDTO>(client.Address) : null,
                Contact = client.Contact != null ? _mapper.Map<ContactDTO>(client.Contact) : null
            }).ToList();
            return Ok(clientsDto);
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetById(int clientId)
        {
            var client = await _clientService.GetByIdAsync(clientId);
            if (client == null) return NotFound();
            var clientDto = new
            {
                client.ClientId,
                client.Name,
                client.CreateDate,
                Address = client.Address != null ? _mapper.Map<AddressDTO>(client.Address) : null,
                Contact = client.Contact != null ? _mapper.Map<ContactDTO>(client.Contact) : null
            };
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO clientCreate)
        {
            try
            {
                var client = _mapper.Map<ClientModel>(clientCreate);
                client.Address = _mapper.Map<AddressModel>(clientCreate.Address);
                client.Contact = _mapper.Map<ContactModel>(clientCreate.Contact);
                var created = await _clientService.AddAsync(clientCreate);
                var createdDto = _mapper.Map<ClientDTO>(created);
                return CreatedAtAction(nameof(GetById), new { clientId = created.ClientId }, createdDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error while creating client.", details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientUpdateDTO clientUpdate)
        {
            try
            {
                var (success, error) = await _clientService.UpdateAsync(clientUpdate);
                if (!success)
                {
                    if (error == "ClientId does not exist.")
                        return NotFound(new { error = error });
                    return BadRequest(new { error = error });
                }
                var updated = await _clientService.GetByIdAsync(clientUpdate.ClientId);
                if (updated == null)
                    return NotFound(new { error = "Client not found after update." });
                var updatedDto = new
                {
                    updated.ClientId,
                    updated.Name,
                    updated.CreateDate,
                    Address = updated.Address != null ? _mapper.Map<AddressDTO>(updated.Address) : null,
                    Contact = updated.Contact != null ? _mapper.Map<ContactDTO>(updated.Contact) : null
                };
                return Ok(updatedDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error while updating client.", details = ex.Message });
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            try
            {
                await _clientService.DeleteAsync(clientId);
                return Ok(new SuccessResponseDTO { Success = true, Message = "Client successfully deleted.", Id = clientId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error while deleting client.", details = ex.Message });
            }
        }
    }
}
