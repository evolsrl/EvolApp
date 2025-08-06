using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/afiliados")]
public class AfiliadosController : ControllerBase
{
    private readonly IAfiliadoRepository _repo;
    public AfiliadosController(IAfiliadoRepository repo) => _repo = repo;

    // GET /api/afiliados/{dni}
    [HttpGet("{dni}")]
    public async Task<ActionResult<AfiliadoDto>> Get(string dni)
    {
        var afi = await _repo.GetByDocumentoAsync(dni);
        return afi is not null ? Ok(afi) : NotFound();
    }

    // POST /api/afiliados/{dni}/enviar-codigo
    [HttpPost("{dni}/enviar-codigo")]
    public async Task<IActionResult> SendCode(string dni)
    {
        await _repo.SendCodeAsync(dni);
        return Ok();
    }

    // POST /api/afiliados/{dni}/validar
    [HttpPost("{dni}/validar")]
    public async Task<IActionResult> Verify(string dni, [FromBody] JsonElement body)
    {
        if (!body.TryGetProperty("codigo", out var c)) return BadRequest();
        var ok = await _repo.VerifyCodeAsync(dni, c.GetString()!);
        return ok ? Ok() : Unauthorized();
    }
}
