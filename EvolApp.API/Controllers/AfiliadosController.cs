using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolApp.Shared.Models;
using EvolAppSocios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

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
}
