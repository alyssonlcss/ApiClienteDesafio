namespace ApiClienteDesafio.DTOs
{
    public class AddressCreateDTO
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int ClientId { get; set; }
    }
}
