using ApiClienteDesafio.Models;
using ApiClienteDesafio.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ApiClienteDesafio.Utils;

namespace ApiClienteDesafio.Validators
{
    public static class ClientValidator
    {
        public static async Task<(bool isValid, string error)> IsBusinessValidAsync(int clientId, AppDbContext context, string? phoneNumber = null)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && !ContactUtils.IsValidCellPhone(phoneNumber))
                return (false, "Número de celular inválido. Formato esperado: DDD + 9 dígitos, ex: 11999999999");
            var exists = await context.Addresses.AnyAsync(a => a.ClientId == clientId);
            if (!exists)
                return (false, "Client does not have an Address.");
            var clientExists = await context.Clients.AnyAsync(c => c.ClientId == clientId);
            if (!clientExists)
                return (false, "ClientId does not exist.");
            return (true, string.Empty);
        }
    }
}