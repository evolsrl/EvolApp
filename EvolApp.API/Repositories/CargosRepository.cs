using Dapper;
using EvolApp.API.Repositories.Interfaces;
using System.Data;
using System.Text.Json;
namespace EvolApp.API.Repositories
{
    public class CargosRepository : ICargosRepository
    {
        private readonly IDbConnection _db;
        public CargosRepository(IDbConnection db) => _db = db;
        public async Task<IEnumerable<dynamic>> ObtenerCargosPendientes(string cuit)
        {
            var cargos = await _db.QueryAsync<dynamic>(
                "EvolAppApiCarCuentasCorrientesSeleccionarPendientesPorAfiliadoDataTable",
                new
                {
                    cuit = cuit
                },
                commandType: CommandType.StoredProcedure
            );

            return cargos.ToList();
        }
        public async Task<IEnumerable<dynamic>> ObtenerCuentaCorrienteCargos(string cuit)
        {
            var cargos = await _db.QueryAsync<dynamic>(
                "EvolAppApiCarCuentasCorrientesSeleccionarCuentaCorrientePorAfiliadoDataTable",
                new
                {
                    cuit = cuit
                },
                commandType: CommandType.StoredProcedure
            );

            return cargos.ToList();
        }
        public async Task<dynamic> ObtenerCuentaCorrienteCargosContratados(string cuit)
        {
            var json = await _db.ExecuteScalarAsync<string>(
                "dbo.EvolApiCarCuentasCorrientesSeleccionarCargosContratadosPorAfiliadoDataTable",
                new { cuit },
                commandType: CommandType.StoredProcedure
            );

            if (string.IsNullOrWhiteSpace(json))
                json = "[]";

            return JsonSerializer.Deserialize<JsonElement>(json);
        }
    }
}