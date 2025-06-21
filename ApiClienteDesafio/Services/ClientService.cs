using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiClienteDesafio.Integration;
using AutoMapper;
using ApiClienteDesafio.Utils;
using System.ComponentModel.DataAnnotations;
using ApiClienteDesafio.Validators;

namespace ApiClienteDesafio.Services
{
    public class ClientService
    {
        private readonly AppDbContext _context;
        private readonly ViaCepIntegration _viaCepIntegration;
        private readonly IMapper _mapper;

        public ClientService(AppDbContext context, ViaCepIntegration viaCepIntegration, IMapper mapper)
        {
            _context = context;
            _viaCepIntegration = viaCepIntegration;
            _mapper = mapper;
        }

        public async Task<List<ClientModel>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .ToListAsync();
        }

        public async Task<ClientModel?> GetByIdAsync(int clientId)
        {
            return await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<ClientModel> AddAsync(ClientModel client)
        {
            if (!ValidationUtils.TryValidateObject(client, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));

            if (client.Contact != null && !ContactUtils.IsValidCellPhone(client.Contact.Number))
                throw new ValidationException("Número de celular inválido. Formato esperado: DDD + 9 dígitos, ex: 11999999999");

            if (client.Address != null)
            {
                var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(client.Address.ZipCode);
                if (viaCepData == null || viaCepData.Erro == "true")
                    throw new ValidationException("CEP inválido ou não encontrado na base ViaCEP.");
                _mapper.Map(viaCepData, client.Address);
            }

            client.CreateDate = DateTime.UtcNow;
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<(bool success, string? error)> UpdateAsync(ClientModel client)
        {
            if (!ValidationUtils.TryValidateObject(client, out var validationResults))
                return (false, string.Join("; ", validationResults));

            var (isValid, businessError) = await ClientValidator.IsBusinessValidAsync(client.ClientId, _context);
            if (!isValid)
                return (false, businessError);

            var existingClient = await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.ClientId == client.ClientId);
            if (existingClient == null)
                throw new System.InvalidOperationException("Client should exist after business validation, but was not found.");

            if (!string.IsNullOrWhiteSpace(client.Name))
                existingClient.Name = client.Name;

            if (client.Address != null)
            {
                var address = client.Address;
                var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(address.ZipCode);
                if (viaCepData == null || viaCepData.Erro == "true")
                    return (false, "CEP inválido ou não encontrado na base ViaCEP.");
                if (existingClient.Address == null)
                {
                    existingClient.Address = _mapper.Map<AddressModel>(address);
                }
                else
                {
                    _mapper.Map(address, existingClient.Address);
                }
                _mapper.Map(viaCepData, existingClient.Address);
            }

            if (client.Contact != null)
            {
                var contact = client.Contact;
                if (!ContactUtils.IsValidCellPhone(contact.Number))
                    return (false, "Número de celular inválido. Formato esperado: DDD + 9 dígitos, ex: 11999999999");
                if (existingClient.Contact == null)
                {
                    existingClient.Contact = _mapper.Map<ContactModel>(contact);
                }
                else
                {
                    _mapper.Map(contact, existingClient.Contact);
                }
            }

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }
    }
}