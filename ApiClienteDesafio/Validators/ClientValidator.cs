using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ApiClienteDesafio.Utils;
using ApiClienteDesafio.DTOs;

namespace ApiClienteDesafio.Validators
{
    public static class ClientValidator
    {
        public static async Task<(bool isValid, string error)> IsBusinessValidCreateAsync(ClientCreateDTO clientCreate, AppDbContext context)
        {
            if (!string.IsNullOrEmpty(clientCreate.Contact.Number) && !ContactUtils.IsValidCellPhone(clientCreate.Contact.Number))
                return (false, "Número de celular inválido. Formato esperado: DDD + 9 dígitos, ex: 11999999999");
            var emailExists = await context.Contacts.AnyAsync(c => c.Email == clientCreate.Contact.Email);
            if (emailExists)
                return (false, "Email already registered for another contact.");
            var phoneExists = await context.Contacts.AnyAsync(c => c.Number == clientCreate.Contact.Number);
            if (phoneExists)
                return (false, "Cell phone number already registered for another contact.");
            return (true, string.Empty);
        }

        public static async Task<(bool isValid, string error)> IsBusinessValidUpdateAsync(ClientUpdateDTO clientUpdate, AppDbContext context)
        {
            var contact = clientUpdate.Contact;
            if (contact == null)
                return (false, "Contact data is required.");

            if (!string.IsNullOrEmpty(contact.Number) && !ContactUtils.IsValidCellPhone(contact.Number))
                return (false, "Invalid cell phone number. Expected format: DDD + 9 digits, e.g., 11999999999");

            if (!string.IsNullOrEmpty(contact.Email))
            {
                bool emailExists = await context.Contacts.AnyAsync(c =>
                    c.Email == contact.Email && c.ClientId != clientUpdate.ClientId);
                if (emailExists)
                    return (false, "Email already registered for another contact.");
            }

            if (!string.IsNullOrEmpty(contact.Number))
            {
                bool phoneExists = await context.Contacts.AnyAsync(c =>
                    c.Number == contact.Number && c.ClientId != clientUpdate.ClientId);
                if (phoneExists)
                    return (false, "Cell phone number already registered for another contact.");
            }

            bool contactExists = await context.Contacts.AnyAsync(c => c.ClientId == clientUpdate.ClientId);
            if (!contactExists)
                return (false, "Contact not found for this client.");

            bool clientExists = await context.Clients.AnyAsync(c => c.ClientId == clientUpdate.ClientId);
            if (!clientExists)
                return (false, "ClientId does not exist.");

            return (true, string.Empty);
        }
    }
}