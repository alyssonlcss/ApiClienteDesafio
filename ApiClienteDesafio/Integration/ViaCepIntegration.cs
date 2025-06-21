using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ApiClienteDesafio.Integration
{
    public class ViaCepIntegration
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ViaCepIntegration(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ViaCepResponse?> GetAddressByCepAsync(string cep)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"https://viacep.com.br/ws/{cep}/json/";
            try
            {
                return await httpClient.GetFromJsonAsync<ViaCepResponse>(url);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }

    public class ViaCepResponse
    {
        public string Logradouro { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Localidade { get; set; } = string.Empty;
        public string Uf { get; set; } = string.Empty;
        public string? Erro { get; set; }
    }
}
