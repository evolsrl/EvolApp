using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.VisualBasic;


[ApiController]
[Route("api/afiliados")]
public class AfiliadosController : ControllerBase
{
    private readonly IAfiliadoRepository _repo;
    public AfiliadosController(IAfiliadoRepository repo) => _repo = repo;
    // GET /api/afiliados/{dni}
    [HttpGet("{documentoOCuit}")]
    public async Task<ActionResult<AfiliadoDto>> ObtenerPorDocumento(string documentoOCuit)
    {
        var afi = await _repo.ObtenerPorDocumento(documentoOCuit);
        return Ok(afi);
    }
    // POST /api/afiliados/{dni}/enviar-codigo
    [HttpPost("{dni}/enviar-codigo")]
    public async Task<IActionResult> EnviarCodigo(string dni)
    {
        await _repo.EnviarCodigo(dni);
        return Ok();
    }
    // POST /api/afiliados/{dni}/verificar
    [HttpPost("{dni}/verificar")]
    public async Task<ActionResult<ResultadoDTO>> VerificarCodigo(string dni, [FromBody] JsonElement body)
    {
        if (!body.TryGetProperty("codigo", out var c)) return BadRequest();
        var ok = await _repo.VerificarCodigo(dni, c.GetString()!);
        return Ok(ok);
    }
    // GET /api/afiliados/{dni}/credencial  (JSON como hoy)
    [HttpGet("{documentoOCuit}/credencial")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<AfiliadoDto>> ObtenerCredencialPorDocumento(string documentoOCuit)
    {
        var afi = await _repo.ObtenerCredencialPorDocumento(documentoOCuit);
        return Ok(afi);
    }
    // SACAR: using Microsoft.VisualBasic;

    [HttpGet("{documentoOCuit}/credencial.png")]
    [Produces("image/png")]
    public async Task<IActionResult> ObtenerCredencialPng(string documentoOCuit, [FromQuery] int page = 0)
    {
        if (page < 0) return BadRequest("Parámetro 'page' inválido.");

        var pdfBytes = await _repo.ObtenerCredencialPdfPorDocumento(documentoOCuit);
        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("No se encontró la credencial en PDF.");

        Response.Headers["Cache-Control"] = "no-store";

        var tmpPng = Path.Combine(Path.GetTempPath(), $"cred_{documentoOCuit}_{page}_{Guid.NewGuid():N}.png");

        try
        {
            await using var ms = new MemoryStream(pdfBytes, writable: false);
            ms.Position = 0;

            // OJO: nombre completo para evitar conflictos
            PDFtoImage.Conversion.SavePng(tmpPng, ms, page);

            var pngBytes = await System.IO.File.ReadAllBytesAsync(tmpPng);
            return File(pngBytes, "image/png", $"Credencial_{documentoOCuit}_p{page}.png");
        }
        finally
        {
            try { if (System.IO.File.Exists(tmpPng)) System.IO.File.Delete(tmpPng); } catch { }
        }
    }

    // POST /api/afiliados/auth/registrar
    [HttpPost("auth/registrar")]
    public async Task<ActionResult<ResultadoDTO>> RegistrarAfiliado([FromBody] JsonElement body)
    {
        try
        {
            if (!body.TryGetProperty("documento", out var documentoProp) ||
                !body.TryGetProperty("username", out var usernameProp) ||
                !body.TryGetProperty("password", out var passwordProp))
            {
                return BadRequest(new ResultadoDTO
                {
                    Exito = Convert.ToBoolean(0),
                    Mensaje = "Faltan datos obligatorios (documento, username o password)."
                });
            }

            var documento = documentoProp.GetString();
            var username = usernameProp.GetString();
            var password = passwordProp.GetString();

            // Validaciones adicionales
            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest(new ResultadoDTO
                {
                    Exito = Convert.ToBoolean(0),
                    Mensaje = "El documento, username y password son obligatorios."
                });
            }

            if (username.Length < 4)
            {
                return BadRequest(new ResultadoDTO
                {
                    Exito = Convert.ToBoolean(0),
                    Mensaje = "El usuario debe tener al menos 4 caracteres."
                });
            }

            if (password.Length < 6)
            {
                return BadRequest(new ResultadoDTO
                {
                    Exito = Convert.ToBoolean(0),
                    Mensaje = "La contraseña debe tener al menos 6 caracteres."
                });
            }

            var resultado = await _repo.RegistrarAfiliado(documento, username, password);

            if (resultado == null)
            {
                return Ok(new ResultadoDTO
                {
                    Exito = Convert.ToBoolean(0),
                    Mensaje = "Error interno del servidor"
                });
            }

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultadoDTO
            {
                Exito = Convert.ToBoolean(0),
                Mensaje = "Error interno del servidor"
            });
        }
    }
    // POST /api/afiliados/auth/login
    [HttpPost("auth/login")]
    public async Task<ActionResult<AfiliadoDto>> LoguearAfiliado([FromBody] JsonElement body)
    {
        if (!body.TryGetProperty("identificador", out var userProp) ||
            !body.TryGetProperty("password", out var passwordProp))
        {
            return BadRequest("Faltan datos obligatorios (identificador o password).");
        }

        var documento = userProp.GetString();
        var password = passwordProp.GetString();

        var resultado = await _repo.LoguearAfiliado(documento!, password!);

        return Ok(resultado);
    }
    // GET /api/afiliados/formas-cobros-afiliados/{id}
    [HttpGet("formas-cobros-afiliados/{documentoOCuit}")]
    public async Task<ActionResult<IEnumerable<FormaCobroDto>>> ObtenerFormasCobrosPorDocumento(string documentoOCuit)
    {
        var formas = await _repo.ObtenerFormasCobrosPorDocumento(documentoOCuit);
        return Ok(formas ?? Enumerable.Empty<FormaCobroDto>());
    }
    [HttpGet("existe/{cuit}")]
    public async Task<IActionResult> Existe(string cuit)
    {
        var res = await _repo.ExisteEvolSocios(cuit);

        if (res == null)
            return NotFound(new { exito = false, mensaje = "Socio no encontrado." });

        return Ok(res);
    }

    /// <summary>
    /// Agrega un afiliado
    /// </summary>
    /// <remarks>
    /// <code>
    ///     {
    ///        "Nombre": "JORGE ANTONIO",
    ///        "Apellido": "REYNOSO",
    ///        "TipoDocumento": "DNI",
    ///        "NumeroDocumento": 27000001,
    ///        "CUIL": 20270000014,
    ///        "NumeroSocio": "101172",
    ///        "MatriculaIAF": 1435,
    ///        "Sexo": "Masculino",
    ///        "FechaNacimiento": "1973-05-31T00:00:00",
    ///        "FechaIngreso": "2009-03-20T00:00:00",
    ///        "TipoPersona": "Fisica",
    ///        "AfiliadoTipo": "Titular",
    ///        "EstadoCivil": "Casado/a",
    ///        "CorreoElectronico": "NO TIENE",
    ///        "Categoria": "Activo",
    ///        "Estado": "Normal",
    ///        "Dependencia": "0",
    ///        "CBU": null,
    ///        "Filial": "Sede Central",
    ///        "Domicilios": [
    ///            {
    ///                "IdDomicilio": 0,
    ///                "IdDomicilioTipo": 1,
    ///                "DomicilioTipo": "Particular",
    ///                "Calle": "BELGRANO",
    ///                "Numero": 124,
    ///                "Piso": 0,
    ///                "Departamento": "0",
    ///                "IdCodigoPostal": 0,
    ///                "IdProvincia": 2,
    ///                "Provincia": "Buenos Aires",
    ///                "Localidad": "SAN ANTONIO DE ARECO",
    ///                "Predeterminado": true,
    ///                "IdEstado": 1,
    ///                "CodigoPostal": "2760"
    ///            }
    ///        ],
    ///        "Telefonos": [
    ///            {
    ///                "IdTelefono": 0,
    ///                "IdTelefonoTipo": 3,
    ///                "Numero": 541127733687,
    ///                "Interno": 0,
    ///                "IdEstado": 1
    ///            }
    ///        ],
    ///        "FormasCobros": [
    ///            {
    ///                "FormaCobro": "Caja",
    ///                "Predeterminado": false,
    ///                "IdEstado": 1,
    ///                "IdFormaCobroAfiliado": 0
    ///            }
    ///        ]
    ///    } 
    ///    </code>
    /// </remarks>>
    /// <param name="json"></param>
    ///     json con los datos del afiliado
    /// <returns></returns>
    [HttpPost("agregar")]
    public async Task<IActionResult> Agregar([FromBody] JsonElement json)
    {
        var res = await _repo.AltaEvolSocios(json.GetRawText());
        return Ok(res);
    }

    /// <summary>
    /// Devuelve los datos de un Afiliado/Socio por su CUIT.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="cuit">CUIT (Clave Única de Identificación Tributaria) Debe ser no nula y una cadena de texto de solo números
    /// </param>
    /// <returns>
    /// </returns>
    [HttpGet("consultar/{cuit}")]
    public async Task<IActionResult> Consultar(string cuit)
    {
        var res = await _repo.ConsultaEvolSocios(cuit);

        if (res == null)
            return NotFound(new { exito = false, mensaje = "Socio no encontrado." });

        return Ok(res);
    }

    /// <summary>
    /// Actualiza un afiliado
    /// </summary>
    /// <remarks>
    /// <code>
    /// {
    ///        "Nombre": "JORGE ANTONIO",
    ///        "Apellido": "REYNOSO",
    ///        "TipoDocumento": "DNI",
    ///        "NumeroDocumento": 27000001,
    ///        "CUIL": 20270000014,
    ///        "NumeroSocio": "101172",
    ///        "MatriculaIAF": 1435,
    ///        "Sexo": "Masculino",
    ///        "FechaNacimiento": "1973-05-31T00:00:00",
    ///        "FechaIngreso": "2009-03-20T00:00:00",
    ///        "TipoPersona": "Fisica",
    ///        "AfiliadoTipo": "Titular",
    ///        "EstadoCivil": "Casado/a",
    ///        "CorreoElectronico": "NO TIENE",
    ///        "Categoria": "Activo",
    ///        "Estado": "Normal",
    ///        "Dependencia": "0",
    ///        "CBU": null,
    ///        "Filial": "Sede Central",
    ///        "Domicilios": [
    ///            {
    ///                "IdDomicilio": 0,
    ///                "IdDomicilioTipo": 1,
    ///                "DomicilioTipo": "Particular",
    ///                "Calle": "BELGRANO",
    ///                "Numero": 124,
    ///                "Piso": 0,
    ///                "Departamento": "0",
    ///                "IdCodigoPostal": 0,
    ///                "IdProvincia": 2,
    ///                "Provincia": "Buenos Aires",
    ///                "Localidad": "SAN ANTONIO DE ARECO",
    ///                "Predeterminado": true,
    ///                "IdEstado": 1,
    ///                "CodigoPostal": "2760"
    ///            }
    ///        ],
    ///        "Telefonos": [
    ///            {
    ///                "IdTelefono": 0,
    ///                "IdTelefonoTipo": 3,
    ///                "Numero": 541127733687,
    ///                "Interno": 0,
    ///                "IdEstado": 1
    ///            }
    ///        ],
    ///        "FormasCobros": [
    ///            {
    ///                "FormaCobro": "Caja",
    ///                "Predeterminado": false,
    ///                "IdEstado": 1,
    ///                "IdFormaCobroAfiliado": 0
    ///            }
    ///        ]
    ///    } 
    /// </code>
    /// </remarks>
    /// <param name="json">
    /// json con los datos del afiliado
    /// </param>
    /// <returns></returns>
    [HttpPost("actualizar")]
    public async Task<IActionResult> Actualizar([FromBody] JsonElement json)
    {
        var res = await _repo.ActualizarEvolSocios(json.GetRawText());

        if (res == null)
            return NotFound(new { exito = false, mensaje = "Socio no encontrado." });

        return Ok(res);
    }

}
