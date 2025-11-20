using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;
using System.Dynamic;
using System.Text.Json;
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

        public async Task<ResultadoDTO> ExisteEvolSocios(string cuit)
        {
            var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                 "EVOLApiAfiAfiliadosExisteSocio",
                 new { cuit },
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<dynamic> AltaEvolSocios(string sociosJson)
        {
            var result = await _db.QuerySingleOrDefaultAsync<dynamic>(
                 "EVOLApiAfiAfiliadosInsertar",
                 new { SociosJson = sociosJson },
                 commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<dynamic> ConsultaEvolSocios(string cuit)
        {
            var row = await _db.QuerySingleOrDefaultAsync<dynamic>(
                "EVOLApiAfiAfiliadosConsultar",
                new { cuit },
                commandType: CommandType.StoredProcedure);

            if (row == null)
                return null;

            // DapperRow implementa IDictionary<string, object>
            var dicOrigen = (IDictionary<string, object>)row;

            dynamic result = new ExpandoObject();
            var dicDestino = (IDictionary<string, object>)result;

            // Copio todas las columnas "simples" tal cual
            foreach (var kv in dicOrigen)
            {
                dicDestino[kv.Key] = kv.Value;
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Domicilio
            if (dicOrigen.TryGetValue("Domicilio", out var domObj) && domObj is string domStr && !string.IsNullOrWhiteSpace(domStr))
            {
                var domicilios = JsonSerializer.Deserialize<List<ExpandoObject>>(domStr, jsonOptions);
                dicDestino["Domicilios"] = domicilios;   // nombre más claro
            }

            // Telefono
            if (dicOrigen.TryGetValue("Telefono", out var telObj) && telObj is string telStr && !string.IsNullOrWhiteSpace(telStr))
            {
                var telefonos = JsonSerializer.Deserialize<List<ExpandoObject>>(telStr, jsonOptions);
                dicDestino["Telefonos"] = telefonos;
            }

            // FormasCobro
            if (dicOrigen.TryGetValue("FormasCobro", out var fcObj) && fcObj is string fcStr && !string.IsNullOrWhiteSpace(fcStr))
            {
                var formasCobro = JsonSerializer.Deserialize<List<ExpandoObject>>(fcStr, jsonOptions);
                dicDestino["FormasCobros"] = formasCobro;
            }

            // Opcional: si no querés seguir exponiendo las columnas crudas
            dicDestino.Remove("Domicilio");
            dicDestino.Remove("Telefono");
            dicDestino.Remove("FormasCobro");

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