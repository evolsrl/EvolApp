using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EvolApp.Shared.DTOs;      // usa los DTOs compartidos
using EvolAppSocios.Services;   // para EmpresaStorageService

namespace EvolAppSocios.Services
{
    public class VotacionApiService
    {
        private readonly HttpClient _client;

        public VotacionApiService(HttpClient? client = null)
        {
            _client = client ?? new HttpClient();
        }

        private string BaseUrl =>
            EmpresaStorageService
                .ObtenerEmpresas()
                .Url?
                .TrimEnd('/')
            ?? string.Empty;

        public async Task<List<EleccionDto>> ObtenerEleccionesAsync()
        {
            // GET /api/elecciones
            var url = $"{BaseUrl}/api/elecciones";

            try
            {
                var elecciones = await _client.GetFromJsonAsync<List<EleccionDto>>(url);
                return elecciones ?? new List<EleccionDto>();
            }
            catch (HttpRequestException)
            {
                return new List<EleccionDto>();
            }
            catch (System.Text.Json.JsonException)
            {
                return new List<EleccionDto>();
            }
        }

        public async Task<List<ListaElectoralDto>> ObtenerListasPorEleccionAsync(string eleccionId)
        {
            // GET /api/elecciones/{id}/listas
            var url = $"{BaseUrl}/api/elecciones/{eleccionId}/listas";

            try
            {
                var listas = await _client.GetFromJsonAsync<List<ListaElectoralDto>>(url);
                return listas ?? new List<ListaElectoralDto>();
            }
            catch (HttpRequestException)
            {
                return new List<ListaElectoralDto>();
            }
            catch (System.Text.Json.JsonException)
            {
                return new List<ListaElectoralDto>();
            }
        }

        public async Task<List<CandidatoDto>> ObtenerCandidatosPorListaAsync(string listaId)
        {
            // GET /api/listas/{listaId}/candidatos
            var url = $"{BaseUrl}/api/listas/{listaId}/candidatos";

            try
            {
                var candidatos = await _client.GetFromJsonAsync<List<CandidatoDto>>(url);
                return candidatos ?? new List<CandidatoDto>();
            }
            catch (HttpRequestException)
            {
                return new List<CandidatoDto>();
            }
            catch (System.Text.Json.JsonException)
            {
                return new List<CandidatoDto>();
            }
        }
    }
}
