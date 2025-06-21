using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ApiClienteDesafio.Utils;
using System.ComponentModel.DataAnnotations;
using ApiClienteDesafio.Validators;
using ApiClienteDesafio.DTOs;

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

        public async Task<bool> UpdateByClientIdAsync(ContactUpdateDTO contactUpdate)
        {
            if (!ValidationUtils.TryValidateObject(contactUpdate, out var validationResults))
                throw new ValidationException(string.Join("; ", validationResults));

            var (isValid, businessError) = await ContactValidator.IsBusinessValidAsync(contactUpdate, _context);
            if (!isValid)
                throw new ValidationException(businessError);

            var existing = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == contactUpdate.ClientId);
            if (existing == null)
                return false;

            _mapper.Map(contactUpdate, existing);

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