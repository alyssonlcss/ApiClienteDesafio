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
            var clientsDto = _mapper.Map<List<ClientDetailDTO>>(clients);
            return Ok(clientsDto);
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetById(int clientId)
        {
            var client = await _clientService.GetByIdAsync(clientId);
            if (client == null) return NotFound();
            var clientDto = _mapper.Map<ClientDetailDTO>(client);
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientCreateDTO clientDTO)
        {
            var client = _mapper.Map<ClientModel>(clientDTO);
            if (!ClientValidator.IsValid(client, out var error))
                return BadRequest(error);

            var created = await _clientService.AddAsync(client);
            var createdDto = _mapper.Map<ClientDTO>(created);
            return CreatedAtAction(nameof(GetById), new { clientId = created.ClientId }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ClientDTO clientDTO)
        {
            var client = _mapper.Map<ClientModel>(clientDTO);
            if (!ClientValidator.IsValid(client, out var error))
                return BadRequest(error);

            await _clientService.UpdateAsync(client);
            return NoContent();
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            await _clientService.DeleteAsync(clientId);
            return NoContent();
        }
    }
}
