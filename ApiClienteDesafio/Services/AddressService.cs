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

        public async Task<(AddressModel? address, string? error)> AddAsync(AddressModel address)
        {
            if (!ValidationUtils.TryValidateObject(address, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));

            var (isValid, businessError) = await AddressValidator.IsBusinessValidAsync(address.ClientId, _context);
            if (!isValid)
                return (null, businessError);

            var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(address.ZipCode);
            if (viaCepData == null || viaCepData.Erro == "true")
                return (null, "Invalid or not found ZipCode (CEP).");
            var viaCepAddress = _mapper.Map<AddressModel>(viaCepData);
            AddressUtils.ApplyViaCepData(address, viaCepAddress);

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return (address, null);
        }

        public async Task<(bool success, string? error)> UpdateByClientIdAsync(AddressModel address)
        {
            if (!ValidationUtils.TryValidateObject(address, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));

            var (isValid, businessError) = await AddressValidator.IsBusinessValidAsync(address.ClientId, _context);
            if (!isValid)
                return (false, businessError);

            var existing = await _context.Addresses.FirstOrDefaultAsync(a => a.ClientId == address.ClientId);
            if (existing == null)
                return (false, "Address not found for this client.");

            var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(address.ZipCode);
            if (viaCepData == null || viaCepData.Erro == "true")
                return (false, "Invalid or not found ZipCode (CEP).");
            var viaCepAddress = _mapper.Map<AddressModel>(viaCepData);
            AddressUtils.ApplyViaCepData(address, viaCepAddress);

            _mapper.Map(address, existing);

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