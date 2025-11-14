using System.Data;
using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories;

public class PrestamoRepository : IPrestamoRepository
{
    private readonly IDbConnection _db;
    public PrestamoRepository(IDbConnection db)
    {
        _db = db;
    }
    public async Task<IEnumerable<PrestamoDto>> ObtenerPorDocumento(string documentoOCuit)
    {
        var p = new DynamicParameters();
        p.Add("@DocumentoOCuit", documentoOCuit);

        try
        {
            var list = await _db.QueryAsync<PrestamoDto>(
                "EvolAppApiPrestamosSeleccionarPorDocumento",
                p,
                commandType: CommandType.StoredProcedure);

            return list;
        }
        catch (Exception ex)
        {
            // Agregar log de error
            throw new Exception("Error al consultar la base de datos.", ex);
        }
    }
    public async Task<PrestamoDetalleDto?> ObtenerPrestamoPorId(string idPrestamo)
    {
        var p = new DynamicParameters();
        p.Add("@IdPrestamo", idPrestamo);

        using var grid = await _db.QueryMultipleAsync(
            "EvolAppApiPrestamosSeleccionarPorIdPrestamo",
            p,
            commandType: CommandType.StoredProcedure);

        // 1) Cabecera (1 fila)
        var cabecera = await grid.ReadFirstOrDefaultAsync<PrestamoDetalleDto>();
        if (cabecera is null) return null;

        // 2) Cuponera (N filas)
        var cuotas = (await grid.ReadAsync<CuotaDto>()).ToList();

        return cabecera with { Cuponera = cuotas };
    }
    public async Task<PrestamoDetalleDto?> ObtenerPrestamoPorDocumento(string documentoOCuit)
    {
        var p = new DynamicParameters();
        p.Add("@DocumentoOCuit", documentoOCuit);

        using var grid = await _db.QueryMultipleAsync(
            "EvolAppApiPrestamosSeleccionarPorIdPrestamo",
            p,
            commandType: CommandType.StoredProcedure);

        // 1) Cabecera (1 fila)
        var cabecera = await grid.ReadFirstOrDefaultAsync<PrestamoDetalleDto>();
        if (cabecera is null) return null;

        // 2) Cuponera (N filas)
        var cuotas = (await grid.ReadAsync<CuotaDto>()).ToList();

        return cabecera with { Cuponera = cuotas };
    }
    public async Task<ResultadoDTO> AltaEvolPrestamos(string json)
    {
        var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
            "EVOLApiPrePrestamosInsertar",
            new { Json = json },
            commandType: CommandType.StoredProcedure);
        return result;
    }
    public async Task<ResultadoDTO> ConsultaEvolPrestamos(string json)
    {
        var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
            "EVOLApiPrePrestamosConsultar",
            new { Json = json },
            commandType: CommandType.StoredProcedure);
        return result;
    }
    public async Task<IEnumerable<PrestamosPlanesDto>> ObtenerPlanesPorFormaCobro(string formaCobro)
    {
        return await _db.QueryAsync<PrestamosPlanesDto>(
            "EvolAppApiPlanesPrestamosSeleccionarPorFormaCobro",
            new { IdFormaCobro = formaCobro },
            commandType: CommandType.StoredProcedure);
    }
}
