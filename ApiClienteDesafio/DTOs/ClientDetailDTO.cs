using System;

namespace ApiClienteDesafio.DTOs
{
    public class ClientDetailDTO
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public AddressDTO Address { get; set; }
        public ContactDTO Contact { get; set; }
    }
}
