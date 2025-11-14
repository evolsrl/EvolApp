using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;
namespace EvolApp.API.Repositories
{
     public class AfiliadoRepository : IAfiliadoRepository
    {
        private readonly IDbConnection _db;
        public AfiliadoRepository(IDbConnection db) => _db = db;
        public async Task<AfiliadoDto?> ObtenerPorDocumento(string documentoOCuit)
        {
            return await _db.QuerySingleOrDefaultAsync<AfiliadoDto>(
                "EvolAppApiAfiliadosSeleccionarPorDNI",
                new { DocumentoOCuit = documentoOCuit },
                commandType: CommandType.StoredProcedure);
        }
        public async Task EnviarCodigo(string documento)
        {
            await _db.ExecuteAsync(
                "EvolAppApiAfiliadosEnviarCodigoVerificacion",
                new { Documento = documento },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<ResultadoDTO> VerificarCodigo(string documento, string codigo)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                "EvolAppApiAfiliadosVerificarCodigo",
                new { Documento = documento, Codigo = codigo },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<ResultadoDTO> ExisteEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                 "EVOLApiAfiAfiliadosExisteSocio",
                 new { SociosJson = sociosJson},
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<AltaAfiliadoResultadoDTO> AltaEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                 "EVOLApiAfiAfiliadosInsertar",
                 new { SociosJson = sociosJson },
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<ConsultaAfiliadosResultadoDTO> ConsultaEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                "EVOLApiAfiAfiliadosConsultar",
                new { SociosJson = sociosJson },
                commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<ResultadoDTO> ActualizarEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                "EVOLApiAfiAfiliadosActualizar",
                new { SociosJson = sociosJson },
                commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}