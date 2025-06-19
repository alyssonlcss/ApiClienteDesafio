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
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clientService;
        private readonly IMapper _mapper;

        public ClientsController(ClientService clientService, IMapper mapper)
        {
            _clientService = clientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            var clientsDto = _mapper.Map<List<ClientDTO>>(clients);
            return Ok(clientsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound();
            var clientDto = _mapper.Map<ClientDTO>(client);
            return Ok(clientDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClientDTO clientDto)
        {
            var client = _mapper.Map<ClientModel>(clientDto);
            if (!ClientValidator.IsValid(client, out var error))
                return BadRequest(error);

            var created = await _clientService.AddAsync(client);
            var createdDto = _mapper.Map<ClientDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClientDTO clientDto)
        {
            var client = _mapper.Map<ClientModel>(clientDto);
            client.Id = id;
            if (!ClientValidator.IsValid(client, out var error))
                return BadRequest(error);

            await _clientService.UpdateAsync(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
