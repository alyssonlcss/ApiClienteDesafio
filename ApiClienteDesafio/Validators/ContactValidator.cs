using ApiClienteDesafio.Models;

namespace ApiClienteDesafio.Validators
{
    public static class ContactValidator
    {
        public static bool IsValid(ContactModel contact, out string error)
        {
            error = string.Empty;
            if (string.IsNullOrWhiteSpace(contact.Number))
            {
                error = "Number is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(contact.Type))
            {
                error = "Type is required.";
                return false;
            }
            return true;
        }
    }
}