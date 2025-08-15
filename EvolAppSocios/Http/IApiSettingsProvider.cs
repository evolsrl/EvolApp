namespace EvolAppSocios.Http;

public interface IApiSettingsProvider
{
    Task<ApiSettings> GetAsync(CancellationToken ct = default);
}
