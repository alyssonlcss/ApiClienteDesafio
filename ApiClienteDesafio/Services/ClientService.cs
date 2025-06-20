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

            _mapper.Map(client, existingClient);

            if (client.Address != null)
            {
                var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(client.Address.ZipCode);
                if (viaCepData != null && viaCepData.Erro != "true")
                {
                    var viaCepAddress = _mapper.Map<AddressModel>(viaCepData);
                    if (existingClient.Address == null)
                    {
                        existingClient.Address = client.Address;
                    }
                    AddressUtils.ApplyViaCepData(existingClient.Address ?? client.Address, viaCepAddress);
                }
                else if (existingClient.Address == null)
                {
                    existingClient.Address = client.Address;
                }
                else
                {
                    _mapper.Map(client.Address, existingClient.Address);
                }
            }

            if (client.Contact != null)
            {
                if (existingClient.Contact == null)
                {
                    existingClient.Contact = client.Contact;
                }
                else
                {
                    _mapper.Map(client.Contact, existingClient.Contact);
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