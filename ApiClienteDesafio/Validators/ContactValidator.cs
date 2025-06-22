using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ApiClienteDesafio.Utils;
using ApiClienteDesafio.DTOs;

namespace ApiClienteDesafio.Validators
{
    public static class ContactValidator
    {
        public static async Task<(bool isValid, string error)> IsBusinessValidAsync(ContactUpdateDTO contactUpdate, AppDbContext context)
        {
            if (!string.IsNullOrEmpty(contactUpdate.Number) && !ContactUtils.IsValidCellPhone(contactUpdate.Number))
                return (false, "Invalid cell phone number. Expected format: DDD + 9 digits, e.g., 11999999999");

            var emailExists = await context.Contacts.AnyAsync(c => c.Email == contactUpdate.Email && c.ClientId != contactUpdate.ClientId);
            if (emailExists && !string.IsNullOrEmpty(contactUpdate.Email))
                return (false, "Email already registered for another contact.");
            var phoneExists = await context.Contacts.AnyAsync(c => c.Number == contactUpdate.Number && c.ClientId != contactUpdate.ClientId);
            if (phoneExists && !string.IsNullOrEmpty(contactUpdate.Number))
                return (false, "Cell phone number already registered for another contact.");
            var exists = await context.Contacts.AnyAsync(c => c.ClientId == contactUpdate.ClientId);
            if (!exists)
                return (false, "A client has no contact. Add one in PUT:/Clients");
            var clientExists = await context.Clients.AnyAsync(c => c.ClientId == contactUpdate.ClientId);
            if (!clientExists)
                return (false, "ClientId does not exist.");
            return (true, string.Empty);
        }
    }
}