public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApiKeyHeaderName = "X-API-KEY";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IConfiguration config)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key faltante");
            return;
        }

        var apiKey = config.GetValue<string>("ApiSettings:MobileApiKey");

        if (!apiKey.Equals(extractedKey))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("API Key inválida");
            return;
        }

        await _next(context);
    }
}
