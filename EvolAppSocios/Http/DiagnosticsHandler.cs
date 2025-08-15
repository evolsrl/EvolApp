public class DiagnosticsHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"[REQ] {request.Method} {request.RequestUri}");
        foreach (var h in request.Headers)
            System.Diagnostics.Debug.WriteLine($"[REQ-H] {h.Key}: {string.Join(",", h.Value)}");
#endif
        var resp = await base.SendAsync(request, ct);
#if DEBUG
        System.Diagnostics.Debug.WriteLine($"[RESP] {(int)resp.StatusCode} {resp.ReasonPhrase}");
        if ((int)resp.StatusCode >= 300 && (int)resp.StatusCode < 400 && resp.Headers.Location != null)
            System.Diagnostics.Debug.WriteLine($"[RESP] Redirect → {resp.Headers.Location}");
#endif
        return resp;
    }
}
