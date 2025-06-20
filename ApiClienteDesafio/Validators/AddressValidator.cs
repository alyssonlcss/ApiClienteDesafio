using ApiClienteDesafio.Models;

namespace ApiClienteDesafio.Validators
{
    public static class AddressValidator
    {
        public static bool IsValid(AddressModel address, out string error)
        {
            error = string.Empty;
            if (address.ClientId <= 0)
            {
                error = "ClientId is required and must be greater than zero.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(address.ZipCode))
            {
                error = "ZipCode is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(address.Number))
            {
                error = "Number is required.";
                return false;
            }
            return true;
        }
    }
}