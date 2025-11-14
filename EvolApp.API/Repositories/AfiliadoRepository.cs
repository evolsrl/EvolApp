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
        public async Task<ResultadoDTO?> RegistrarAfiliado(string documento, string username, string password)
        {
            try
            {
                var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                    "EvolAppApiAfiliadosRegistrarAfiliado",
                    new { Documento = documento, Username = username, Password = password },
                    commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                return new ResultadoDTO
                {
                    Exito = Convert.ToBoolean(0),
                    Mensaje = "Error al procesar el registro"
                };
            }
        }
        public async Task<AfiliadoDto?> LoguearAfiliado(string documento, string password)
        {
            var result = await _db.QuerySingleOrDefaultAsync<AfiliadoDto>(
                "EvolAppApiAfiliadosLoguearAfiliado",
                new { Documento = documento, Password = password },
                commandType: CommandType.StoredProcedure);
            return result;
        }
        public async Task<IEnumerable<FormaCobroDto>> ObtenerFormasCobrosPorDocumento(string documentoOCuit)
        {
            return await _db.QueryAsync<FormaCobroDto>(
                "EvolAppApiAfiliadosSeleccionarFormaCobroPorDNI",
                new { DocumentoOCuit = documentoOCuit },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<ResultadoDTO> ExisteEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                 "EVOLApiAfiAfiliadosExisteSocio",
                 new { SociosJson = sociosJson},
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<ResultadoDTO> AltaEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                 "EVOLApiAfiAfiliadosInsertar",
                 new { SociosJson = sociosJson },
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<ResultadoDTO> ConsultaEvolSocios(string sociosJson)
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