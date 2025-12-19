using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;
using System.Dynamic;
using System.Text.Json;
namespace EvolApp.API.Repositories
{
    public class CargosRepository : ICargosRepository
    {
        private readonly IDbConnection _db;
        public CargosRepository(IDbConnection db) => _db = db;
        public async Task<List<CargosDto>> ObtenerCargosPendientes(string documento)
        {
            var cargos = await _db.QueryAsync<CargosDto>(
                "EvolAppApiCarCuentasCorrientesSeleccionarPendientesPorAfiliadoDataTable",
                new
                {
                    NumeroDocumento = documento
                },
                commandType: CommandType.StoredProcedure
            );

            return cargos.ToList();
        }
        public async Task<List<CargosDto>> ObtenerCuentaCorrienteCargos(string documento)
        {
            var cargos = await _db.QueryAsync<CargosDto>(
                "EvolAppApiCarCuentasCorrientesSeleccionarCuentaCorrientePorAfiliadoDataTable",
                new
                {
                    NumeroDocumento = documento
                },
                commandType: CommandType.StoredProcedure
            );

            return cargos.ToList();
        }
    }
}