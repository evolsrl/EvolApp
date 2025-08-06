using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using EvolApp.Shared.DTOs;  // usa el DTO compartido

namespace EvolAppSocios.Services
{
    public class EmpresaApiService
    {
        private readonly HttpClient _client;

        public EmpresaApiService(HttpClient? client = null)
        {
            _client = client ?? new HttpClient
            {
                BaseAddress = new Uri("https://erp.evol.com.ar/")
            };
            _client.DefaultRequestHeaders.Add("X-API-KEY", "evolapp-supersecreta-123");
        }

        public async Task<EmpresaDto?> ObtenerEmpresa(string cuit)
        {
            // construye la ruta relativa al BaseAddress
            const string route = "evolappapi/api/empresas/obtener";

            // envía el body como JSON
            var requestBody = new { Cuit = cuit };
            var response = await _client.PostAsJsonAsync(route, requestBody);

            if (!response.IsSuccessStatusCode)
                return null;

            // deserializa directamente al DTO, ignorando mayúsculas/minúsculas
            return await response.Content.ReadFromJsonAsync<EmpresaDto>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
