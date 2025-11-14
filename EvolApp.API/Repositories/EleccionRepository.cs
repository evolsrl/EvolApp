using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;

namespace EvolApp.API.Repositories;
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

    public async Task<ResultadoDTO> GetValidarVotoAsync(string listaId, string documento)
    {
        var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
        "EvolAppApiEleccionesValidarVoto",
        new { ListaId = listaId, Documento = documento },
        commandType: CommandType.StoredProcedure);
        return result;
    }
    
    public async Task<ResultadoDTO> GetVotarAsync(string listaId, string documento)
    {
        var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
        "EvolAppApiEleccionesVotar",
        new { ListaId = listaId, Documento = documento },
        commandType: CommandType.StoredProcedure);
        return result;
    }
}
