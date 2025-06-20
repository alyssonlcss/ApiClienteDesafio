using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Services
{
    public class ClientService
    {
        private readonly AppDbContext _context;

        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientModel>> GetAllAsync()
        {
            return await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .ToListAsync();
        }

        public async Task<ClientModel> GetByIdAsync(int clientId)
        {
            return await _context.Clients
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<ClientModel> AddAsync(ClientModel client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task UpdateAsync(ClientModel client)
        {
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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