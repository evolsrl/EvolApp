using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace EvolAppSocios.Http;

public abstract class BaseApiService
{
    protected readonly HttpClient Http;
    private readonly ISecurityContext _security;
    private readonly JsonSerializerOptions _json;

    protected BaseApiService(HttpClient http, ISecurityContext security)
    {
        Http = http;
        _security = security;
        _json = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    // --- URI helper: arma absoluta si es necesario
    protected async Task<Uri> BuildUriAsync(string pathOrAbsolute, CancellationToken ct = default)
    {
        // 1) Si ya me pasaron una URL http/https absoluta, úsala tal cual
        if (Uri.TryCreate(pathOrAbsolute, UriKind.Absolute, out var abs) &&
            (abs.Scheme == Uri.UriSchemeHttp || abs.Scheme == Uri.UriSchemeHttps))
        {
            System.Diagnostics.Debug.WriteLine($"[HTTP] URL absoluta http(s): {abs}");
            return abs;
        }

        // 2) Caso contrario, combino con la base (http/https)
        var baseUri = Http.BaseAddress ?? await _security.GetBaseAddressAsync(ct);

        // Quitar el slash inicial para no “resetear” el path y perder el /api
        var relative = pathOrAbsolute.TrimStart('/');

        var final = new Uri(baseUri, relative);
        System.Diagnostics.Debug.WriteLine($"[HTTP] Base: {baseUri} + Rel: {relative} => {final}");
        return final;
    }

    // --- Helpers HTTP tipados

    protected async Task<T?> GetAsync<T>(string path, CancellationToken ct = default)
    {
        var uri = await BuildUriAsync(path, ct);
        using var resp = await Http.GetAsync(uri, ct);
        await EnsureSuccess(resp);
        return await resp.Content.ReadFromJsonAsync<T>(_json, ct);
    }

    protected async Task<TRes?> PostAsync<TReq, TRes>(string path, TReq payload, CancellationToken ct = default)
    {
        var uri = await BuildUriAsync(path, ct);
        var content = new StringContent(JsonSerializer.Serialize(payload, _json), Encoding.UTF8, "application/json");
        using var resp = await Http.PostAsync(uri, content, ct);
        await EnsureSuccess(resp);
        return await resp.Content.ReadFromJsonAsync<TRes>(_json, ct);
    }

    protected async Task PostAsync<TReq>(string path, TReq payload, CancellationToken ct = default)
    {
        var uri = await BuildUriAsync(path, ct);
        var content = new StringContent(JsonSerializer.Serialize(payload, _json), Encoding.UTF8, "application/json");
        using var resp = await Http.PostAsync(uri, content, ct);
        await EnsureSuccess(resp);
    }

    protected async Task<TRes?> PutAsync<TReq, TRes>(string path, TReq payload, CancellationToken ct = default)
    {
        var uri = await BuildUriAsync(path, ct);
        var content = new StringContent(JsonSerializer.Serialize(payload, _json), Encoding.UTF8, "application/json");
        using var resp = await Http.PutAsync(uri, content, ct);
        await EnsureSuccess(resp);
        return await resp.Content.ReadFromJsonAsync<TRes>(_json, ct);
    }

    protected async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        var uri = await BuildUriAsync(path, ct);
        using var resp = await Http.DeleteAsync(uri, ct);
        await EnsureSuccess(resp);
    }

    // --- Manejo de errores centralizado
    private static async Task EnsureSuccess(HttpResponseMessage resp)
    {
        if (resp.IsSuccessStatusCode) return;

        string body = string.Empty;
        try { body = await resp.Content.ReadAsStringAsync(); } catch { /* ignore */ }

        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            throw new HttpRequestException($"401 Unauthorized. Body: {body}");
        if (resp.StatusCode == HttpStatusCode.Forbidden)
            throw new HttpRequestException($"403 Forbidden. Body: {body}");

        throw new HttpRequestException($"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {body}");
    }
}
