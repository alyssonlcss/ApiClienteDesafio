using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClienteDesafio.Models
{
    public class ContactModel
    {
        public int ContactId { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }
        public int ClientId { get; set; }
        public ClientModel Client { get; set; }
    }
}