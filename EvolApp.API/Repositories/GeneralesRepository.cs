using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;

namespace EvolApp.API.Repositories;
public class GeneralesRepository : IGeneralesRepository
{
    private readonly IDbConnection _db;
    public GeneralesRepository(IDbConnection db) => _db = db;

    public async Task<IEnumerable<FormaCobroDto>> GetAllAsync()
    {
        return await _db.QueryAsync<FormaCobroDto>(
            "EvolAppApiEleccionesSeleccionar",
            commandType: CommandType.StoredProcedure);
    }
    public async Task<EmpresaDto> GetEmpresas()
    {
        return await _db.QuerySingleOrDefaultAsync<EmpresaDto>(
            "EvolAppApiObtenerEmpresas",
            commandType: CommandType.StoredProcedure);
    }
    public async Task<ResultadoDTO> GetEndpoint(string cuit)
    {
        return await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
            "EvolAppApiObtenerEndpointPorCuit",
            new { Cuit = cuit },
            commandType: CommandType.StoredProcedure);
    }
}