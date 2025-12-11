using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;
using System.Dynamic;
using System.Text.Json;
namespace EvolApp.API.Repositories
{
     public class AhorroRepository : IAhorroRepository
    {
        private readonly IDbConnection _db;
        public AhorroRepository(IDbConnection db) => _db = db;
        
        public async Task<IEnumerable<dynamic>> ProgramaPuntosCajasAhorrosPuntos()
        {
            var result = await _db.QueryAsync<dynamic>(
                 "ApiProgramaPuntosCajasAhorrosPuntos",
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<dynamic>> ProgramaPuntosNumerosSorteos(DateTime fecha)
        {
            var result = await _db.QueryAsync<dynamic>(
                 "ApiProgramaPuntosNumerosSorteos ",
                 new { Fecha = fecha },
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<dynamic>> ProgramaPuntosCodigosConceptos()
        {
            var result = await _db.QueryAsync<dynamic>(
                 "ApiProgramaPuntosCodigosConceptos",
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<dynamic>> ProgramaPuntosCodigosParticipaciones()
        {
            var result = await _db.QueryAsync<dynamic>(
                 "ApiProgramaPuntosCodigosParticipaciones",
                 commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}