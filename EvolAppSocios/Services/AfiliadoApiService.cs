using EvolAppSocios.Http;
using EvolApp.Shared.DTOs;

namespace EvolAppSocios.Services;

public class AfiliadoApiService : BaseApiService
{
    public AfiliadoApiService(HttpClient http, ISecurityContext security) : base(http, security) { }

    public Task<AfiliadoDto?> ObtenerAfiliado(string documento, CancellationToken ct = default)
        => GetAsync<AfiliadoDto>($"/afiliados/{documento}", ct);

    public Task EnviarCodigo(string documento, CancellationToken ct = default)
        => PostAsync($"/afiliados/{documento}/enviar-codigo", new { documento }, ct);

    public Task<bool> VerificarCodigo(string documento, string codigo, CancellationToken ct = default)
        => PostAsync<object, bool>($"/afiliados/{documento}/verificar", new { documento, codigo }, ct);
}
