using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cargos")]
public class CargosController : ControllerBase
{
    private readonly ICargosRepository _repo;
    public CargosController(ICargosRepository repo) => _repo = repo;
    // GET /api/cargos/pendientes/{documento}
    [HttpGet("pendientes/{documento}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<List<CargosDto>>> ObtenerCargosPendientes([FromRoute] string documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCargosPendientes(documento);

        return Ok(result);
    }
    // GET /api/cargos/cuenta-corriente/{documento}
    [HttpGet("cuenta-corriente/{documento}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<List<CargosDto>>> ObtenerCuentaCorrienteCargos([FromRoute] string documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCuentaCorrienteCargos(documento);

        return Ok(result);
    }
}
