using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

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
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> ObtenerPorDocumento([FromRoute] string documentoOCuit)
    {
        if (string.IsNullOrWhiteSpace(documentoOCuit))
            return BadRequest("Documento requerido.");

        var data = await _repo.ObtenerPorDocumento(documentoOCuit);
        return Ok(data);
    }
    // GET /api/prestamosDetalles/id/{id}
    [HttpGet("prestamosDetalles/id/{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
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
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<PrestamoDetalleDto>> ObtenerPrestamoPorDocumento([FromRoute] string documentoOCuit)
    {
        if (string.IsNullOrWhiteSpace(documentoOCuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var data = await _repo.ObtenerPrestamoPorDocumento(documentoOCuit);
        if (data is null) return NotFound();

        return Ok(data);
    }
    // GET /api/prestamosDetalles/documento/{documentoOCuit}
    [HttpGet("prestamosDetalles/planes/{formaCobro}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<IEnumerable<PrestamosPlanesDto>>> ObtenerPlanesPorFormaCobro(string formaCobro)
    {
        var planes = await _repo.ObtenerPlanesPorFormaCobro(formaCobro);
        return Ok(planes ?? Enumerable.Empty<PrestamosPlanesDto>());
    }

    [HttpPost("prestamos/agregar")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Agregar([FromBody] JsonElement json)
    {
        var res = await _repo.AltaEvolPrestamos(json.GetRawText());
        return Ok(res);
    }
    [HttpGet("prestamos/consultar/{cuit}")]
    public async Task<IActionResult> Consultar(string cuit)
    {
        var prestamos = await _repo.ConsultaEvolPrestamos(cuit);

        if (prestamos == null || !prestamos.Any())
            return NotFound(new { exito = false, mensaje = "No se encontraron préstamos para el CUIT indicado." });

        return Ok(prestamos);
    }
}