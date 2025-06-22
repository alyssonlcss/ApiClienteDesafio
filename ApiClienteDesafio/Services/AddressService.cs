using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ApiClienteDesafio.Integration;
using AutoMapper;
using ApiClienteDesafio.Utils;
using System.ComponentModel.DataAnnotations;
using ApiClienteDesafio.Validators;
using ApiClienteDesafio.DTOs;

namespace ApiClienteDesafio.Services
{
    public class AddressService
    {
        private readonly AppDbContext _context;
        private readonly ViaCepIntegration _viaCepIntegration;
        private readonly IMapper _mapper;

        public AddressService(AppDbContext context, ViaCepIntegration viaCepIntegration, IMapper mapper)
        {
            _context = context;
            _viaCepIntegration = viaCepIntegration;
            _mapper = mapper;
        }

        public async Task<AddressModel?> GetByClientIdAsync(int clientId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.ClientId == clientId);
        }

        public async Task<(bool success, string? error)> UpdateByClientIdAsync(AddressUpdateDTO addressUpdate)
        {
            if (!ValidationUtils.TryValidateObject(addressUpdate, out var validationResults))
                return (false, string.Join("; ", validationResults));

            var (isValid, businessError) = await AddressValidator.IsBusinessValidAsync(addressUpdate.ClientId, _context);
            if (!isValid)
                return (false, businessError);

            var existing = await _context.Addresses.FirstOrDefaultAsync(a => a.ClientId == addressUpdate.ClientId);

            var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(addressUpdate.ZipCode);
            if (viaCepData == null || viaCepData.Erro == "true")
                return (false, "Invalid or not found ZipCode (CEP).");

            _mapper.Map(addressUpdate, existing);
            _mapper.Map(viaCepData, existing);

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task DeleteByClientIdAsync(int clientId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.ClientId == clientId);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }
}