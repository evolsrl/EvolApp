using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using System.Data;
namespace EvolApp.API.Repositories
{
     public class AfiliadoRepository : IAfiliadoRepository
    {
        private readonly IDbConnection _db;
        public AfiliadoRepository(IDbConnection db) => _db = db;

        public async Task<AfiliadoDto?> GetByDocumentoAsync(string documento)
        {
            return await _db.QuerySingleOrDefaultAsync<AfiliadoDto>(
                "EvolAppApiAfiliadosSeleccionarPorDNI",
                new { Documento = documento },
                commandType: CommandType.StoredProcedure);
        }

        public async Task SendCodeAsync(string documento)
        {
            await _db.ExecuteAsync(
                "EvolAppApiAfiliadosEnviarCodigoVerificacion",
                new { Documento = documento },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> VerifyCodeAsync(string documento, string codigo)
        {
            var result = await _db.QuerySingleAsync<int>(
                "EvolAppApiAfiliadosVerificarCodigo",
                new { Documento = documento, Codigo = codigo },
                commandType: CommandType.StoredProcedure);
            return result == 1;
        }
    }
}
