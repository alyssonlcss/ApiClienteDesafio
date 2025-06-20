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

        public async Task<ContactModel> AddAsync(int clientId, ContactModel contact)
        {
            // Verifica se já existe Contact para este ClientId
            var exists = await _context.Contacts.AnyAsync(c => c.ClientId == clientId);
            if (exists)
                return null; // Ou retorne um erro, se preferir

            // Verifica se o Client existe
            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == clientId);
            if (!clientExists)
                return null; // Ou retorne um erro, se preferir

            contact.ClientId = clientId;
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> UpdateByClientIdAsync(int clientId, ContactModel contact)
        {
            var existing = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (existing == null)
                return false;

            contact.ContactId = existing.ContactId;
            contact.ClientId = clientId;
            _context.Entry(contact).State = EntityState.Modified;
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