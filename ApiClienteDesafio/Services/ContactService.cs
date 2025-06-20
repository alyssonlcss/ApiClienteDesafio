using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Services
{
    public class ContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ContactModel> GetByClientIdAsync(int clientId)
        {
            return await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<ContactModel> AddAsync(ContactModel contact)
        {
            var exists = await _context.Contacts.AnyAsync(c => c.ClientId == contact.ClientId);
            if (exists)
                return null; 
            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == contact.ClientId);
            if (!clientExists)
                return null; 

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> UpdateByClientIdAsync(ContactModel contact)
        {
            var existing = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == contact.ClientId);
            if (existing == null)
                return false;

            existing.Number = contact.Number;
            existing.Type = contact.Type;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteByClientIdAsync(int clientId)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }
    }
}