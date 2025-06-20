using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.Utils
{
    public static class ValidationUtils
    {
        public static bool TryValidateObject<T>(T obj, out List<ValidationResult> results)
        {
            var context = new ValidationContext(obj, null, null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(obj, context, results, true);
        }
    }
}
