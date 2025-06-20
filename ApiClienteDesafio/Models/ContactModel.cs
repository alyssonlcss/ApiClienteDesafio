using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClienteDesafio.Models
{
    public class ContactModel
    {
        public int ContactId { get; set; }
        [Required]
        [StringLength(20)]
        public string Number { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public ClientModel? Client { get; set; }
    }
}