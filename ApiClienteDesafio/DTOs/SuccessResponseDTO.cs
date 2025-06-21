namespace ApiClienteDesafio.DTOs
{
    public class SuccessResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? Id { get; set; }
    }
}
