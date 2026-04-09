using EvolApp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cargos")]
public class CargosController : ControllerBase
{
    private readonly ICargosRepository _repo;
    public CargosController(ICargosRepository repo) => _repo = repo;
    [HttpGet("pendientes/{cuit}")]
    public async Task<IActionResult> ObtenerCargosPendientes([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCargosPendientes(cuit);

        return Ok(result);
    }
    /// <summary>
    /// Listado de cargos/servicios devengados en la cuenta corriente de un afiliado
    /// </summary>
    /// <remarks>
    /// Devuelve un listado cargos/servicios devengados en la cuenta corriente.
    /// </remarks>
    /// <returns></returns>
    [HttpGet("cuenta-corriente/{cuit}")]
    public async Task<IActionResult> ObtenerCuentaCorrienteCargos([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.ObtenerCuentaCorrienteCargos(cuit);

        return Ok(result);
    }
    /// <summary>
    /// Cargos/Servicos pendientes de pago de un afiliado
    /// </summary>
    /// <remarks>
    /// Devuelve un listado de cargos/servicois que el socio tiene contratados.
    /// </remarks>
    /// <returns></returns>
    [HttpGet("cargos-contratados/{cuit}")]
    public async Task<IActionResult> ObtenerCuentaCorrienteCargosContratados([FromRoute] string cuit)
    {
        if (string.IsNullOrWhiteSpace(cuit))
            return BadRequest("CUIT o DNI requerido.");

        var result = await _repo.ObtenerCuentaCorrienteCargosContratados(cuit);

        return Ok(result);
    }
}
