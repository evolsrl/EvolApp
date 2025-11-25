// JsonBodyExampleOperationFilter.cs
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace EvolApp.API.Swagger;

public class JsonBodyExampleOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var attr = context.MethodInfo
                          .GetCustomAttributes(true)
                          .OfType<JsonBodyExampleAttribute>()
                          .FirstOrDefault();

        if (attr == null)
            return;

        if (operation.RequestBody == null)
            return;

        if (!operation.RequestBody.Content.TryGetValue("application/json", out var mediaType))
            return;

        mediaType.Example = new OpenApiString(attr.Example);
    }
}
