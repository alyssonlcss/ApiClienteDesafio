using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.DTOs
{
    public class ClientUpdateDTO
    {
        [Required]
        public int ClientId { get; set; }
        public string? Name { get; set; } = string.Empty;
        
        public AddressUpdateDTO? Address { get; set; }
        
        public ContactUpdateDTO? Contact { get; set; }
    }
}
