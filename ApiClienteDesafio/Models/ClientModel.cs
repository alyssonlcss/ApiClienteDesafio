using System;
using System.ComponentModel.DataAnnotations;

namespace ApiClienteDesafio.Models
{
    public class ClientModel
    {
        public int ClientId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        [Required]
        public required AddressModel Address { get; set; }
        [Required]
        public required ContactModel Contact { get; set; }
    }
}