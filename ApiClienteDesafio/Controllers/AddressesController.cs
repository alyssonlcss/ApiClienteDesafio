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
    [Route("addresses")]
    public class AddressesController : ControllerBase
    {
        private readonly AddressService _addressService;
        private readonly IMapper _mapper;

        public AddressesController(AddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var address = await _addressService.GetByClientIdAsync(clientId);
            if (address == null) return NotFound();
            var addressDto = _mapper.Map<AddressDTO>(address);
            return Ok(addressDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddressCreateDTO addressDTO)
        {
            var address = _mapper.Map<AddressModel>(addressDTO);
            var (created, error) = await _addressService.AddAsync(address);
            if (error != null) return BadRequest(error);
            var createdDto = _mapper.Map<AddressDTO>(created);
            return CreatedAtAction(nameof(GetByClientId), new { clientId = address.ClientId }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AddressCreateDTO addressDTO)
        {
            var address = _mapper.Map<AddressModel>(addressDTO);
            var (success, error) = await _addressService.UpdateByClientIdAsync(address);
            if (!success) return BadRequest(error);
            return NoContent();
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            await _addressService.DeleteByClientIdAsync(clientId);
            return NoContent();
        }
    }
}