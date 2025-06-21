using System.Text.RegularExpressions;

namespace ApiClienteDesafio.Utils
{
    public static class ContactUtils
    {
        public static bool IsValidCellPhone(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return false;
            return Regex.IsMatch(number, @"^\d{2}9\d{8}$");
        }
    }
}
