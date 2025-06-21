namespace ApiClienteDesafio.DTOs
{
    public class ContactUpdateDTO
    {
        public int ClientId { get; set; }
        public string? Number { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
}