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
        public static async Task<(bool isValid, string error)> IsBusinessValidAsync(ClientCreateDTO clientCreate, AppDbContext context)
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
    }
}