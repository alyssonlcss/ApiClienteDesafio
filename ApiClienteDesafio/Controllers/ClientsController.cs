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
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO clientDTO)
        {
            try
            {
                var client = _mapper.Map<ClientModel>(clientDTO);
                client.Address = _mapper.Map<AddressModel>(clientDTO.Address);
                client.Contact = _mapper.Map<ContactModel>(clientDTO.Contact);
                var created = await _clientService.AddAsync(client);
                var createdDto = _mapper.Map<ClientDTO>(created);
                return CreatedAtAction(nameof(GetById), new { clientId = created.ClientId }, createdDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao criar cliente.", details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientUpdateDTO clientDTO)
        {
            try
            {
                var client = _mapper.Map<ClientModel>(clientDTO);
                var (success, error) = await _clientService.UpdateAsync(client);
                if (!success)
                {
                    if (error == "ClientId does not exist.")
                        return NotFound(new { error = error });
                    return BadRequest(new { error = error });
                }
                var updated = await _clientService.GetByIdAsync(client.ClientId);
                if (updated == null)
                    return NotFound(new { error = "Cliente não encontrado após atualização." });
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
                return StatusCode(500, new { error = "Erro interno ao atualizar cliente.", details = ex.Message });
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            try
            {
                await _clientService.DeleteAsync(clientId);
                return Ok(new SuccessResponseDTO { Success = true, Message = "Cliente removido com sucesso.", Id = clientId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao remover cliente.", details = ex.Message });
            }
        }
    }
}
