using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EvolApp.API.Swagger
{
    public class MostrarSoloSchemasPorConfigDocumentFilter : IDocumentFilter
    {
        private readonly IConfiguration _config;

        public MostrarSoloSchemasPorConfigDocumentFilter(IConfiguration config)
        {
            _config = config;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Sección padre "Swagger"
            var swaggerSection = _config.GetSection("Swagger");
            if (!swaggerSection.Exists())
                return;

            // Sub-sección "IncluirSchemas"
            var incluirSchemasSection = swaggerSection.GetSection("IncluirSchemas");

            // 👉 Hay que detectar si la CLAVE existe, aunque el array esté vacío
            var tieneClaveIncluirSchemas = swaggerSection
                .GetChildren()
                .Any(s => string.Equals(s.Key, "IncluirSchemas", StringComparison.OrdinalIgnoreCase));

            // Si la clave ni siquiera está definida => no filtramos nada (mostrar todos)
            if (!tieneClaveIncluirSchemas)
                return;

            var schemasConfig = incluirSchemasSection.Get<string[]>() ?? Array.Empty<string>();

            // ✅ Clave definida pero array vacío => borrar TODOS los schemas
            if (schemasConfig.Length == 0)
            {
                swaggerDoc.Components.Schemas.Clear();
                return;
            }

            // Caso normal: whitelist
            var permitidos = schemasConfig
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var schemasNoPermitidos = swaggerDoc.Components.Schemas
                .Where(s => !permitidos.Contains(s.Key))
                .Select(s => s.Key)
                .ToList();

            foreach (var nombre in schemasNoPermitidos)
            {
                swaggerDoc.Components.Schemas.Remove(nombre);
            }
        }
    }
}
