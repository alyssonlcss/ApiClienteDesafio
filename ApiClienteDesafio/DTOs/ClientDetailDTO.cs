using System;

namespace ApiClienteDesafio.DTOs
{
    public class ClientDetailDTO
    {
        public int ClientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public AddressDTO? Address { get; set; }
        public ContactDTO? Contact { get; set; }
    }
}
