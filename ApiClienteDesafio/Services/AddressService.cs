using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiClienteDesafio.Services
{
    public class AddressService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public AddressService(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<AddressModel> GetByClientIdAsync(int clientId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.ClientId == clientId);
        }

        public async Task<(AddressModel address, string error)> AddAsync(AddressModel address)
        {
            var exists = await _context.Addresses.AnyAsync(a => a.ClientId == address.ClientId);
            if (exists)
                return (null, "A client can only have one address.");

            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == address.ClientId);
            if (!clientExists)
                return (null, "ClientId does not exist.");

            var (success, error) = await FillAddressFromViaCep(address.ZipCode, address);
            if (!success)
                return (null, error);

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return (address, null);
        }

        public async Task<(bool success, string error)> UpdateByClientIdAsync(AddressModel address)
        {
            var existing = await _context.Addresses.FirstOrDefaultAsync(a => a.ClientId == address.ClientId);
            if (existing == null)
                return (false, "Address not found for this client.");

            // Atualiza apenas os campos necessários do objeto já rastreado
            var (success, error) = await FillAddressFromViaCep(address.ZipCode, existing);
            if (!success)
                return (false, error);

            existing.ZipCode = address.ZipCode;
            existing.Number = address.Number;

            await _context.SaveChangesAsync();
            return (true, null);
        }

        private async Task<(bool success, string error)> FillAddressFromViaCep(string zipCode, AddressModel address)
        {
            var viaCepUrl = $"https://viacep.com.br/ws/{zipCode}/json/";
            var viaCepData = await _httpClient.GetFromJsonAsync<ViaCepResponse>(viaCepUrl);

            if (viaCepData == null || viaCepData.Erro == "true")
                return (false, "Invalid or not found ZipCode (CEP).");

            address.Street = viaCepData.Logradouro;
            address.Neighborhood = viaCepData.Bairro;
            address.City = viaCepData.Localidade;
            address.State = viaCepData.Uf;
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

        public class ViaCepResponse
        {
            public string Logradouro { get; set; }
            public string Bairro { get; set; }
            public string Localidade { get; set; }
            public string Uf { get; set; }
            public string Erro { get; set; }
        }
    }
}