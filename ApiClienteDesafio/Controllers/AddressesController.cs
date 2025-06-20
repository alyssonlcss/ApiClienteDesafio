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
            if (!ValidationUtils.TryValidateObject(address, out var validationResults))
                return BadRequest(string.Join("; ", validationResults.Select(v => v.ErrorMessage)));
            var (created, serviceError) = await _addressService.AddAsync(address);
            if (serviceError != null) return BadRequest(serviceError);
            var createdDto = _mapper.Map<AddressDTO>(created);
            return CreatedAtAction(nameof(GetByClientId), new { clientId = address.ClientId }, createdDto);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AddressCreateDTO addressDTO)
        {
            var address = _mapper.Map<AddressModel>(addressDTO);
            if (!ValidationUtils.TryValidateObject(address, out var validationResults))
                return BadRequest(string.Join("; ", validationResults.Select(v => v.ErrorMessage)));
            var (success, serviceError) = await _addressService.UpdateByClientIdAsync(address);
            if (!success) return BadRequest(serviceError);
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