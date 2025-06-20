using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClienteDesafio.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int ClientId { get; set; }
        public ClientModel Client { get; set; }
    }
}