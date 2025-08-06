using EvolApp.Shared.DTOs;   // <-- usar el DTO compartido
using EvolApp.Shared.Models;
using EvolAppSocios.Services; // para EmpresaStorageService
using EvolAppSocios.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EvolAppSocios.Services
{
    public class MenuApiService
    {
        private readonly HttpClient _client;

        public MenuApiService(HttpClient? client = null)
        {
            _client = client ?? new HttpClient();
        }

        private string BaseUrl =>
            EmpresaStorageService
                .ObtenerEmpresas()
                .Url?
                .TrimEnd('/')
            ?? string.Empty;

        public async Task<List<OpcionMenu>> ObtenerOpciones(string dni)
        {
            // GET /api/menu/{dni}
            var url = $"{BaseUrl}/api/menu/{dni}";

            try
            {
                var dto = await _client.GetFromJsonAsync<List<OpcionMenuDto>>(url) ?? new();
                var modelos = dto.Select(d => new OpcionMenu
                {
                    Clave = d.Clave,
                    Nombre = d.Nombre,
                    Pagina = d.Pagina
                }).ToList();

                MenuMapper.MapearTipos(modelos);
                return modelos;
            }
            catch (HttpRequestException)
            {
                // manejar fallos de red
                return new List<OpcionMenu>();
            }
            catch (System.Text.Json.JsonException)
            {
                // manejar errores de deserialización
                return new List<OpcionMenu>();
            }
        }
    }
}

