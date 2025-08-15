using System.Text.Json;
using Microsoft.Maui.Storage;

namespace EvolAppSocios.Http;

public class ApiSettingsProvider : IApiSettingsProvider
{
    private readonly Lazy<Task<ApiSettings>> _lazy;

    public ApiSettingsProvider()
    {
        _lazy = new Lazy<Task<ApiSettings>>(LoadAsync);
    }

    public Task<ApiSettings> GetAsync(CancellationToken ct = default) => _lazy.Value;

    private static async Task<ApiSettings> LoadAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        using var doc = JsonDocument.Parse(json);

        if (!doc.RootElement.TryGetProperty("ApiSettings", out var apiEl))
            throw new InvalidOperationException("Falta la sección 'ApiSettings' en appsettings.json");

        // Leer campos a mano (sin reflexión, evita problemas de trimming en Android)
        string? baseUrl = apiEl.TryGetProperty("BaseUrl", out var baseUrlEl) ? baseUrlEl.GetString() : null;
        string? apiKey = apiEl.TryGetProperty("ApiKey", out var apiKeyEl) ? apiKeyEl.GetString() : null;

        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("ApiSettings.BaseUrl es nulo o vacío");

        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out var baseUri) ||
            (baseUri.Scheme != Uri.UriSchemeHttp && baseUri.Scheme != Uri.UriSchemeHttps))
            throw new InvalidOperationException($"ApiSettings.BaseUrl inválida: {baseUrl}");

        return new ApiSettings
        {
            BaseUrl = baseUrl,
            ApiKey = apiKey
        };
    }
}