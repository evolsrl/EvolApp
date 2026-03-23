using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cargos")]
public class CargosController : ControllerBase
{
    private readonly ICargosRepository _repo;
    public CargosController(ICargosRepository repo) => _repo = repo;
    [HttpGet("pendientes/{cuit}")]
    public async Task<ActionResult<List<CargosDto>>> ObtenerCargosPendientes([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCargosPendientes(cuit);

        return Ok(result);
    }
    [HttpGet("cuenta-corriente/{cuit}")]
    public async Task<ActionResult<List<CargosDto>>> ObtenerCuentaCorrienteCargos([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCuentaCorrienteCargos(cuit);

        return Ok(result);
    }
}
