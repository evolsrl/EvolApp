using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using System.Data;

public class EleccionRepository : IEleccionRepository
{
    private readonly IDbConnection _db;
    public EleccionRepository(IDbConnection db) => _db = db;

    public async Task<IEnumerable<EleccionDto>> GetAllAsync()
    {
        return await _db.QueryAsync<EleccionDto>(
            "EvolAppApiEleccionesSeleccionar",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ListaElectoralDto>> GetListasAsync(string eleccionId)
    {
        return await _db.QueryAsync<ListaElectoralDto>(
            "EvolAppApiListasEleccionesSeleccionarPorEleccion",
            new { EleccionId = eleccionId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<CandidatoDto>> GetCandidatosAsync(string listaId)
    {
        return await _db.QueryAsync<CandidatoDto>(
            "EvolAppApiListasEleccionesPostulantesSeleccionarPorLista",
            new { ListaId = listaId },
            commandType: CommandType.StoredProcedure);
    }
}
