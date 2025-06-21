using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.DTOs
{
    public class ContactCreateDTO
    {
        [Required]
        public string Number { get; set; } = string.Empty;
        
        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
