using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ApiClienteDesafio.Utils;
using System.ComponentModel.DataAnnotations;
using ApiClienteDesafio.Validators;

namespace ApiClienteDesafio.Services
{
    public class ContactService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ContactService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContactModel?> GetByClientIdAsync(int clientId)
        {
            return await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<ContactModel?> AddAsync(ContactModel contact)
        {
            if (!ValidationUtils.TryValidateObject(contact, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));

            var (isValid, businessError) = await ContactValidator.IsBusinessValidAsync(contact, _context);
            if (!isValid)
                throw new ValidationException(businessError);

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
            if (!ValidationUtils.TryValidateObject(contact, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));

            var (isValid, businessError) = await ContactValidator.IsBusinessValidAsync(contact, _context);
            if (!isValid)
                throw new ValidationException(businessError);

            var existing = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == contact.ClientId);
            if (existing == null)
                return false;

            _mapper.Map(contact, existing);

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