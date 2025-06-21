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
            if (!ContactUtils.IsValidCellPhone(contactUpdate.Number))
                return (false, "Número de celular inválido. Formato esperado: DDD + 9 dígitos, ex: 11999999999");
            var exists = await context.Contacts.AnyAsync(c => c.ClientId == contactUpdate.ClientId);
            if (!exists)
                return (false, "A client can only have one contact.");
            var clientExists = await context.Clients.AnyAsync(c => c.ClientId == contactUpdate.ClientId);
            if (!clientExists)
                return (false, "ClientId does not exist.");
            return (true, string.Empty);
        }
    }
}