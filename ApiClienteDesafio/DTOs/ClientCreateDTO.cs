using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.DTOs
{
    public class ClientCreateDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateTime? CreateDate { get; set; } 
        [Required]
        public required AddressCreateDTO Address { get; set; }
        [Required]
        public required ContactCreateDTO Contact { get; set; }
    }
}
