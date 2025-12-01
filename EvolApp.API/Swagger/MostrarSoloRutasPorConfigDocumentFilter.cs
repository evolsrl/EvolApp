using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EvolApp.API.Swagger
{
    public class MostrarSoloRutasPorConfigDocumentFilter : IDocumentFilter
    {
        private readonly IConfiguration _config;

        public MostrarSoloRutasPorConfigDocumentFilter(IConfiguration config)
        {
            _config = config;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Sección padre "Swagger"
            var swaggerSection = _config.GetSection("Swagger");
            if (!swaggerSection.Exists())
                return;

            // Sub-sección "IncluirRutas"
            var incluirRutasSection = swaggerSection.GetSection("IncluirRutas");

            // 👉 Hay que detectar si la CLAVE existe, aunque el array esté vacío
            var tieneClaveIncluirRutas = swaggerSection
                .GetChildren()
                .Any(s => string.Equals(s.Key, "IncluirRutas", StringComparison.OrdinalIgnoreCase));

            // Si la clave ni siquiera está definida => no filtramos nada (mostrar todas las rutas)
            if (!tieneClaveIncluirRutas)
                return;

            var rutasConfig = incluirRutasSection.Get<string[]>() ?? Array.Empty<string>();

            // ✅ Clave definida pero array vacío => borrar TODAS las rutas
            if (rutasConfig.Length == 0)
            {
                swaggerDoc.Paths.Clear();
                return;
            }

            var patrones = rutasConfig
                .Select(r => Normalizar(r))
                .ToList();

            bool EsPermitida(string path)
            {
                var p = Normalizar(path);

                return patrones.Any(pattern =>
                    pattern.EndsWith("/*", StringComparison.OrdinalIgnoreCase)
                        ? p.StartsWith(pattern[..^2], StringComparison.OrdinalIgnoreCase) // prefijo
                        : p.Equals(pattern, StringComparison.OrdinalIgnoreCase));          // exacto
            }

            // Tomamos todas las rutas que NO están en la whitelist
            var pathsNoPermitidos = swaggerDoc.Paths
                .Where(p => !EsPermitida(p.Key))
                .Select(p => p.Key)
                .ToList();

            // Las borramos del documento
            foreach (var path in pathsNoPermitidos)
            {
                swaggerDoc.Paths.Remove(path);
            }
        }

        private static string Normalizar(string? ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
                return "/";

            ruta = ruta.Trim();

            // Aseguramos que empiece con "/"
            if (!ruta.StartsWith("/"))
                ruta = "/" + ruta;

            // Sacar barra final (salvo que sea solo "/")
            if (ruta.Length > 1 && ruta.EndsWith("/"))
                ruta = ruta[..^1];

            return ruta;
        }
    }
}
