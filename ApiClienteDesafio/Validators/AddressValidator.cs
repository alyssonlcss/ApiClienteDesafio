using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Validators
{
    public static class AddressValidator
    {
        public static bool IsBusinessValid(AddressModel address, out string error)
        {
            error = string.Empty;
            return true;
        }

        public static async Task<(bool isValid, string error)> IsBusinessValidAsync(int clientId, AppDbContext context)
        {
            var exists = await context.Addresses.AnyAsync(a => a.ClientId == clientId);
            if (!exists)
                return (false, "A Client does not have an address.");
            var clientExists = await context.Clients.AnyAsync(c => c.ClientId == clientId);
            if (!clientExists)
                return (false, "ClientId does not exist.");
            return (true, string.Empty);
        }
    }
}