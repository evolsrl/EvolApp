namespace EvolAppSocios.Http;

public class SecurityHeaderHandler : DelegatingHandler
{
    private readonly ISecurityContext _security;

    public SecurityHeaderHandler(ISecurityContext security) => _security = security;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Remove("X-API-KEY");
        var apiKey = await _security.GetApiKeyAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(apiKey))
            request.Headers.Add("X-API-KEY", apiKey);

        return await base.SendAsync(request, cancellationToken);
    }
}
