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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AddressUpdateDTO addressUpdate)
        {
            try
            {
                var (success, serviceError) = await _addressService.UpdateByClientIdAsync(addressUpdate);
                if (!success)
                    return BadRequest(new { error = serviceError });
                var updated = await _addressService.GetByClientIdAsync(addressUpdate.ClientId);
                if (updated == null)
                    return NotFound(new { error = "Endereço não encontrado após atualização." });
                var updatedDto = _mapper.Map<AddressDTO>(updated);
                return Ok(updatedDto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro interno ao atualizar endereço.", details = ex.Message });
            }
        }

        [HttpDelete("{clientId}")]
        public async Task<IActionResult> Delete(int clientId)
        {
            await _addressService.DeleteByClientIdAsync(clientId);
            return Ok(new SuccessResponseDTO { Success = true, Message = "Endereço removido com sucesso.", Id = clientId });
        }
    }
}