using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiClienteDesafio.Models
{
    public class AddressModel
    {
        public int AddressId { get; set; }
        [Required]
        [StringLength(150)]
        public string Street { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string Number { get; set; } = string.Empty;
        [StringLength(100)]
        public string? Complement { get; set; }
        [Required]
        [StringLength(100)]
        public string Neighborhood { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;
        [Required]
        [StringLength(2)]
        public string? State { get; set; }
        [Required]
        [StringLength(10)]
        public string ZipCode { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public ClientModel? Client { get; set; }
    }
}