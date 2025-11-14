using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EvolApp.API.Controllers;

[ApiController]
[Route("api")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoRepository _repo;
    public PrestamosController(IPrestamoRepository repo)
    {
        _repo = repo;
    }
    // GET /api/prestamos/documento/{documentoOCuit}
    [HttpGet("prestamos/documento/{documentoOCuit}")]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> ObtenerPorDocumento([FromRoute] string documentoOCuit)
    {
        if (string.IsNullOrWhiteSpace(documentoOCuit))
            return BadRequest("Documento requerido.");

        var data = await _repo.ObtenerPorDocumento(documentoOCuit);
        return Ok(data);
    }
    // GET /api/prestamosDetalles/id/{id}
    [HttpGet("prestamosDetalles/id/{id}")]
    public async Task<ActionResult<PrestamoDetalleDto>> ObtenerPrestamoPorId([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Id de préstamo requerido.");

        var data = await _repo.ObtenerPrestamoPorId(id);
        if (data is null) return NotFound();

        return Ok(data);
    }
    // GET /api/prestamosDetalles/documento/{documentoOCuit}
    [HttpGet("prestamosDetalles/documento/{documentoOCuit}")]
    public async Task<ActionResult<PrestamoDetalleDto>> ObtenerPrestamoPorDocumento([FromRoute] string documentoOCuit)
    {
        if (string.IsNullOrWhiteSpace(documentoOCuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var data = await _repo.ObtenerPrestamoPorDocumento(documentoOCuit);
        if (data is null) return NotFound();

        return Ok(data);
    }

    [Authorize]
    [HttpPost("AltaEvolPrestamos")]
    public async Task<IActionResult> AltaEvolPrestamos(Json json)
    {
        var res = await _repo.AltaEvolPrestamos(json.GetRawText());
        return Ok(res);
    }
    [Authorize]
    [HttpPost("ConsultaEvolPrestamos")]
    public async Task<IActionResult> ConsultaEvolPrestamos(Json json)
    {
        var res = await _repo.ConsultaEvolPrestamos(json.GetRawText());
        return Ok(res);
    }
}