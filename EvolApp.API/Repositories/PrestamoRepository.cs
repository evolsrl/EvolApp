using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using System.Data;
using System.Dynamic;
using System.Text.Json;

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
    public async Task<IEnumerable<CuotaProximaDto>> ObtenerCuotasProximasVencerPorDocumento(string documentoOCuit){
        var p = new DynamicParameters();
        p.Add("@DocumentoOCuit", documentoOCuit);

        try
        {
            var list = await _db.QueryAsync<CuotaProximaDto>(
                "EvolAppApiPrestamosSeleccionarCuotasProximasVencerPorDocumento",
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
    public async Task<IEnumerable<PrestamosPlanesDto>> ObtenerPlanesPrestamoSimulacion()
    {
        return await _db.QueryAsync<PrestamosPlanesDto>(
            "EvolAppApiSelectPrePrestamosPlanes",
            commandType: CommandType.StoredProcedure);
    }
    public async Task<IEnumerable<PrestamosPlanesDto>> ObtenerPrestamosCantidadCuotasSimulacion(string idPlan)
    {
        return await _db.QueryAsync<PrestamosPlanesDto>(
            "EvolAppApiSelectPrePrestamosCantidadCuotas",
            new { IdPlan = idPlan },
            commandType: CommandType.StoredProcedure);
    }
    public async Task<List<CuotaDto>> ArmarCuponera(
    int? idTipoOperacion,
    int idPrestamoPlan,
    int? idPrestamoPlanTasa,
    int cantidadCuotas,
    string importeSolicitado
)
    {
        var cuotas = await _db.QueryAsync<CuotaDto>(
            "EvolAppApiPrePrestamosArmarCuponera",
            new
            {
                IdTipoOperacion = idTipoOperacion,
                IdPrestamoPlan = idPrestamoPlan,
                IdPrestamoPlanTasa = idPrestamoPlanTasa,
                CantidadCuotas = cantidadCuotas,
                ImporteSolicitado = importeSolicitado
            },
            commandType: CommandType.StoredProcedure
        );

        return cuotas.ToList();
    }
    public async Task<IEnumerable<PrestamosPlanesDto>> ObtenerPlanesPorFormaCobro(string formaCobro)
    {
        return await _db.QueryAsync<PrestamosPlanesDto>(
            "EvolAppApiPlanesPrestamosSeleccionarPorFormaCobro",
            new { IdFormaCobro = formaCobro },
            commandType: CommandType.StoredProcedure);
    }
    public async Task<dynamic> AltaEvolPrestamos(string json)
    {
        var result = await _db.QuerySingleOrDefaultAsync<dynamic>(
            "EVOLApiPrePrestamosInsertar",
            new { Json = json },
            commandType: CommandType.StoredProcedure);
        return result;
    }
    public async Task<IEnumerable<dynamic>> ConsultaEvolPrestamos(string cuit)
    {
        var rows = await _db.QueryAsync<dynamic>(
            "EVOLApiPrePrestamosConsultar",
            new { Cuil = cuit },
            commandType: CommandType.StoredProcedure);

        var lista = new List<dynamic>();

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        foreach (var row in rows)
        {
            var dicOrigen = (IDictionary<string, object>)row;

            dynamic item = new ExpandoObject();
            var dicDestino = (IDictionary<string, object>)item;

            // Copio todas las columnas tal cual
            foreach (var kv in dicOrigen)
            {
                // Manejar DBNull para que no explote el JSON
                dicDestino[kv.Key] = kv.Value is DBNull ? null : kv.Value;
            }

            // Parseo JSON de Cuotas (que viene como NVARCHAR(MAX) del SP)
            if (dicOrigen.TryGetValue("Cuotas", out var cuotasObj)
                && cuotasObj is string cuotasStr
                && !string.IsNullOrWhiteSpace(cuotasStr))
            {
                try
                {
                    var cuotas = JsonSerializer.Deserialize<List<ExpandoObject>>(cuotasStr, jsonOptions);

                    // Si deserializa bien, reemplazo la propiedad
                    if (cuotas != null)
                        dicDestino["Cuotas"] = cuotas;
                }
                catch
                {
                    // Si por alguna razón falla el parseo, dejo el string original
                    // para no romper el endpoint.
                }
            }

            lista.Add(item);
        }

        return lista;
    }
    
}
