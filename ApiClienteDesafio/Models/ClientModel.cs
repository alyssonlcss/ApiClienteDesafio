using System;
using System.Collections.Generic;

namespace ApiClienteDesafio.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public AddressModel Address { get; set; }
        public ContactModel Contact { get; set; } 
    }
}