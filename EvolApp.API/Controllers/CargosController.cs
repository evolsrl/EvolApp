using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cargos")]
public class CargosController : ControllerBase
{
    private readonly ICargosRepository _repo;
    public CargosController(ICargosRepository repo) => _repo = repo;
    // GET /api/cargos/pendientes/{cuit}
    [HttpGet("pendientes/{cuit}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<List<CargosDto>>> ObtenerCargosPendientes([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCargosPendientes(cuit);

        return Ok(result);
    }
    // GET /api/cargos/cuenta-corriente/{cuit}
    [HttpGet("cuenta-corriente/{cuit}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<List<CargosDto>>> ObtenerCuentaCorrienteCargos([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCuentaCorrienteCargos(cuit);

        return Ok(result);
    }
}
