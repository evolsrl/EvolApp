using EvolApp.Shared.DTOs;
using EvolAppSocios.Http;

public class VotacionApiService : BaseApiService
{
    public VotacionApiService(HttpClient http, ISecurityContext cfg) : base(http, cfg) { }

    public Task<List<EleccionDto>?> ObtenerEleccionesAsync(CancellationToken ct = default)
        => GetAsync<List<EleccionDto>>("/elecciones", ct);

    public Task<List<ListaElectoralDto>?> ObtenerListasPorEleccionAsync(string idEleccion, CancellationToken ct = default)
        => GetAsync<List<ListaElectoralDto>>($"/elecciones/{idEleccion}/listas", ct);

    public Task<List<CandidatoDto>?> ObtenerCandidatosPorListaAsync(string idLista, CancellationToken ct = default)
        => GetAsync<List<CandidatoDto>>($"/listas/{idLista}/candidatos", ct);

    public Task<bool> RegistrarVotoAsync(string documento, int idLista, CancellationToken ct = default)
        => PostAsync<object, bool>("/votos", new { documento, idLista }, ct);
}