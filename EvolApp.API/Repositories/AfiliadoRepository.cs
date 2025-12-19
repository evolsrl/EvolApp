using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;
using System.Dynamic;
using System.Reflection;
using System.Text;
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
        public async Task<AfiliadoDto?> ObtenerCredencialPorDocumento(string documentoOCuit)
        {
            return await _db.QuerySingleOrDefaultAsync<AfiliadoDto>(
                "EvolAppApiAfiliadosSeleccionarCredencialPorDNI",
                new { DocumentoOCuit = documentoOCuit },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<byte[]?> ObtenerCredencialPdfPorDocumento(string documentoOCuit)
        {
            var afi = await ObtenerCredencialPorDocumento(documentoOCuit);
            if (afi == null) return null;

            // 1) Si en algún entorno ya viniera como bytes
            var bytes = TryGetBytesProperty(afi, "CredencialPdf", "PdfBytes", "Pdf");
            if (bytes != null && bytes.Length > 0) return bytes;

            // 2) Caso más común: base64 en un string (como tu front ya maneja)
            var raw = TryGetStringProperty(afi,
                "CredencialDigital", "credencialDigital",
                "PdfBase64", "pdfBase64",
                "Pdf", "pdf");

            if (string.IsNullOrWhiteSpace(raw))
                return null;

            // Si viene URL, no se puede convertir (salvo que quieras que el backend la descargue)
            if (raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("La credencial llegó como URL; se requiere PDF embebido (base64 o varbinary).");

            var base64 = ExtractBase64(raw);

            // Validación mínima: PDF suele empezar con JVBERi0 en base64
            if (!base64.StartsWith("JVBERi0", StringComparison.Ordinal))
                throw new InvalidOperationException("El contenido no parece ser un PDF en base64.");

            return Convert.FromBase64String(base64);
        }

        private static byte[]? TryGetBytesProperty(object obj, params string[] names)
        {
            var t = obj.GetType();
            foreach (var n in names)
            {
                var p = t.GetProperty(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (p == null) continue;
                if (p.PropertyType == typeof(byte[]))
                    return (byte[]?)p.GetValue(obj);
            }
            return null;
        }

        private static string? TryGetStringProperty(object obj, params string[] names)
        {
            var t = obj.GetType();
            foreach (var n in names)
            {
                var p = t.GetProperty(n, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (p == null) continue;
                if (p.PropertyType == typeof(string))
                    return (string?)p.GetValue(obj);
            }
            return null;
        }

        private static string ExtractBase64(string raw)
        {
            raw = raw.Trim();

            // data:application/pdf;base64,...
            var idx = raw.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
            if (raw.StartsWith("data:", StringComparison.OrdinalIgnoreCase) && idx >= 0)
                raw = raw[(idx + "base64,".Length)..];

            // Si viene “envuelto” (xml/texto), recortar desde el header típico de PDF en base64
            var jv = raw.IndexOf("JVBERi0", StringComparison.Ordinal);
            if (jv > 0) raw = raw.Substring(jv);

            // Dejar solo chars base64
            var sb = new StringBuilder(raw.Length);
            foreach (var ch in raw)
            {
                if ((ch >= 'A' && ch <= 'Z') ||
                    (ch >= 'a' && ch <= 'z') ||
                    (ch >= '0' && ch <= '9') ||
                    ch == '+' || ch == '/' || ch == '=')
                    sb.Append(ch);
            }

            var clean = sb.ToString();

            // padding
            var mod = clean.Length % 4;
            if (mod == 2) clean += "==";
            else if (mod == 3) clean += "=";
            else if (mod == 1) throw new FormatException("Base64 inválido (longitud incorrecta).");

            return clean;
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
        public async Task<ResultadoDTO> ResetearContrasenia(string documento, string codigo, string password)
        {
            try
            {
                var result = await _db.QuerySingleOrDefaultAsync<ResultadoDTO>(
                    "EvolAppApiAfiliadosResetearContrasenia",
                    new { Documento = documento, Codigo = codigo, Password = password },
                    commandType: CommandType.StoredProcedure);

                return result ?? new ResultadoDTO
                {
                    Exito = false,
                    Mensaje = "Error interno del servidor"
                };
            }
            catch (Exception)
            {
                // Log ex
                return new ResultadoDTO
                {
                    Exito = false,
                    Mensaje = "Error al procesar el reseteo de contraseña"
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