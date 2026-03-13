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
        public async Task<List<CargosDto>> ObtenerCargosPendientes(string cuit)
        {
            var cargos = await _db.QueryAsync<CargosDto>(
                "EvolAppApiCarCuentasCorrientesSeleccionarPendientesPorAfiliadoDataTable",
                new
                {
                    cuit = cuit
                },
                commandType: CommandType.StoredProcedure
            );

            return cargos.ToList();
        }
        public async Task<List<CargosDto>> ObtenerCuentaCorrienteCargos(string cuit)
        {
            var cargos = await _db.QueryAsync<CargosDto>(
                "EvolAppApiCarCuentasCorrientesSeleccionarCuentaCorrientePorAfiliadoDataTable",
                new
                {
                    cuit = cuit
                },
                commandType: CommandType.StoredProcedure
            );

            return cargos.ToList();
        }
    }
}