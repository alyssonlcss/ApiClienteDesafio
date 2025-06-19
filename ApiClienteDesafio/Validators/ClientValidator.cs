using ApiClienteDesafio.Models;

namespace ApiClienteDesafio.Validators
{
    public static class ClientValidator
    {
        public static bool IsValid(ClientModel client, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(client.Name))
            {
                error = "Name is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(client.Cpf))
            {
                error = "CPF is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(client.Email))
            {
                error = "Email is required.";
                return false;
            }
            if (client.BirthDate > DateTime.Now.AddYears(-18))
            {
                error = "Client must be at least 18 years old.";
                return false;
            }
            return true;
        }
    }
}