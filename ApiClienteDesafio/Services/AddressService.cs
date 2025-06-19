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

        public async Task<List<AddressModel>> GetByClientIdAsync(int clientId)
        {
            return await _context.Addresses.Where(a => a.ClientId == clientId).ToListAsync();
        }

        public async Task<AddressModel> GetByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }

        public async Task<(AddressModel address, string error)> AddAsync(AddressModel address)
        {
            var (success, error) = await FillAddressFromViaCep(address.ZipCode, address);
            if (!success)
                return (null, error);

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return (address, null);
        }

        public async Task<(bool success, string error)> UpdateAsync(AddressModel address)
        {
            var (success, error) = await FillAddressFromViaCep(address.ZipCode, address);
            if (!success)
                return (false, error);

            _context.Entry(address).State = EntityState.Modified;
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

        public async Task DeleteAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
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