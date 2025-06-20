using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Validators
{
    public static class ContactValidator
    {
        public static async Task<(bool isValid, string error)> IsBusinessValidAsync(ContactModel contact, AppDbContext context)
        {
            var exists = await context.Contacts.AnyAsync(c => c.ClientId == contact.ClientId && c.ContactId != contact.ContactId);
            if (exists)
                return (false, "A client can only have one contact.");
            var clientExists = await context.Clients.AnyAsync(c => c.ClientId == contact.ClientId);
            if (!clientExists)
                return (false, "ClientId does not exist.");
            return (true, string.Empty);
        }
    }
}