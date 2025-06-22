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
using ApiClienteDesafio.DTOs;

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

        public async Task<ClientModel> AddAsync(ClientCreateDTO clientCreate)
        {
            if (!ValidationUtils.TryValidateObject(clientCreate, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));
            
            var (isValid, businessError) = await ClientValidator.IsBusinessValidCreateAsync(clientCreate, _context);
            if (!isValid)
                throw new ValidationException(businessError);

            var newClient = _mapper.Map<ClientModel>(clientCreate);
            if (clientCreate.Address != null)
            {
                var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(clientCreate.Address.ZipCode);
                if (viaCepData == null || viaCepData.Erro == "true")
                    throw new ValidationException("Invalid or not found ZipCode (CEP).");
                _mapper.Map(viaCepData, newClient.Address);
            }
            
            newClient.CreateDate = DateTime.UtcNow;
            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();
            return newClient;
        }

        public async Task<(bool success, string? error)> UpdateAsync(ClientUpdateDTO clientUpdate)
        {

            if (!ValidationUtils.TryValidateObject(clientUpdate, out var validationResults))
                return (false, string.Join("; ", validationResults));
            
            if (clientUpdate.Contact != null)
            {
                var (isValidContact, businessErrorContact) = await ClientValidator.IsBusinessValidUpdateAsync(clientUpdate, _context);
                if (!isValidContact)
                    return (false, businessErrorContact);
            }

            var existingClient = await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.ClientId == clientUpdate.ClientId);
            if (existingClient == null)
                return (false, "Client not found.");

            if (!string.IsNullOrWhiteSpace(clientUpdate.Name))
                existingClient.Name = clientUpdate.Name;

            if (clientUpdate.Address != null)
            {
                var address = clientUpdate.Address;
                var viaCepData = await _viaCepIntegration.GetAddressByCepAsync(address.ZipCode);
                if (viaCepData == null || viaCepData.Erro == "true")
                    return (false, "Invalid or not found ZipCode (CEP).");
                existingClient.Address = _mapper.Map<AddressModel>(address);
                
                _mapper.Map(viaCepData, existingClient.Address);
            }

            if (clientUpdate.Contact != null)
            {
                if (existingClient.Contact == null)
                    existingClient.Contact = _mapper.Map<ContactModel>(clientUpdate.Contact);
                else
                {
                    if (!string.IsNullOrWhiteSpace(clientUpdate.Contact.Number))
                    existingClient.Contact.Number = clientUpdate.Contact.Number;
                    if (!string.IsNullOrWhiteSpace(clientUpdate.Contact.Type))
                        existingClient.Contact.Type = clientUpdate.Contact.Type;
                    if (!string.IsNullOrWhiteSpace(clientUpdate.Contact.Email))
                        existingClient.Contact.Email = clientUpdate.Contact.Email;
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