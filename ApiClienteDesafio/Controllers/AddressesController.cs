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
    public class AddressesController : ControllerBase
    {
        private readonly AddressService _addressService;
        private readonly IMapper _mapper;

        public AddressesController(AddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var addresses = await _addressService.GetByClientIdAsync(clientId);
            var addressesDto = _mapper.Map<List<AddressDTO>>(addresses);
            return Ok(addressesDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var address = await _addressService.GetByIdAsync(id);
            if (address == null) return NotFound();
            var addressDto = _mapper.Map<AddressDTO>(address);
            return Ok(addressDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddressDTO addressDto)
        {
            var address = _mapper.Map<AddressModel>(addressDto);
            if (!AddressValidator.IsValid(address, out var error))
                return BadRequest(error);

            var (created, viaCepError) = await _addressService.AddAsync(address);
            if (viaCepError != null)
                return BadRequest(viaCepError);

            var createdDto = _mapper.Map<AddressDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddressDTO addressDto)
        {
            var address = _mapper.Map<AddressModel>(addressDto);
            address.Id = id;
            if (!AddressValidator.IsValid(address, out var error))
                return BadRequest(error);

            var (success, viaCepError) = await _addressService.UpdateAsync(address);
            if (!success)
                return BadRequest(viaCepError);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _addressService.DeleteAsync(id);
            return NoContent();
        }
    }
}