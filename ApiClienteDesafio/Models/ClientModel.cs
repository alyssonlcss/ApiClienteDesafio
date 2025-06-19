using System;
using System.Collections.Generic;

namespace ApiClienteDesafio.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }

        public ICollection<AddressModel> Addresses { get; set; }
        public ICollection<ContactModel> Contacts { get; set; }
    }
}