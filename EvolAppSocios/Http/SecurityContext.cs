namespace EvolAppSocios.Http;

public interface ISecurityContext
{
    Task<string?> GetApiKeyAsync(CancellationToken ct = default);
    Task<string?> GetBearerTokenAsync(CancellationToken ct = default);
    Task<Uri> GetBaseAddressAsync(CancellationToken ct = default);
}

public class SecurityContext : ISecurityContext
{
    private readonly IApiSettingsProvider _provider;

    public SecurityContext(IApiSettingsProvider provider) => _provider = provider;

    public async Task<string?> GetApiKeyAsync(CancellationToken ct = default)
        => (await _provider.GetAsync(ct)).ApiKey;

    public Task<string?> GetBearerTokenAsync(CancellationToken ct = default)
        => Task.FromResult<string?>(null);

    public async Task<Uri> GetBaseAddressAsync(CancellationToken ct = default)
        => new((await _provider.GetAsync(ct)).BaseUrl!);
}
