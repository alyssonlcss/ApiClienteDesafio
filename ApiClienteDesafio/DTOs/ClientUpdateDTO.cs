using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.DTOs
{
    public class ClientUpdateDTO
    {
        [Required]
        public int ClientId { get; set; }
        public string? Name { get; set; } = string.Empty;
        
        public AddressCreateDTO? Address { get; set; }
        
        public ContactCreateDTO? Contact { get; set; }
    }
}
