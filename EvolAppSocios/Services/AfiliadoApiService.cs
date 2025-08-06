using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Services; // para EmpresaStorageService

namespace EvolAppSocios.Services
{
    public class AfiliadoApiService
    {
        private readonly HttpClient _client;

        public AfiliadoApiService(HttpClient? client = null)
        {
            _client = client ?? new HttpClient();
        }

        private string BaseUrl =>
            EmpresaStorageService.ObtenerEmpresas().Url?.TrimEnd('/') ?? string.Empty;

        public async Task<AfiliadoDto?> ObtenerAfiliado(string dni)
        {
            // GET /api/afiliados/{dni}
            return await _client
                .GetFromJsonAsync<AfiliadoDto>($"{BaseUrl}/api/afiliados/{dni}");
        }

        public async Task<bool> EnviarCodigo(string dni)
        {
            // POST /api/afiliados/{dni}/enviar-codigo
            var response = await _client
                .PostAsync($"{BaseUrl}/api/afiliados/{dni}/enviar-codigo", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> VerificarCodigo(string dni, string codigo)
        {
            // POST /api/afiliados/{dni}/validar
            var content = JsonContent.Create(new { codigo });
            var response = await _client
                .PostAsync($"{BaseUrl}/api/afiliados/{dni}/validar", content);
            return response.IsSuccessStatusCode;
        }
    }
}
