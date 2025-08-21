using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/elecciones")]
public class EleccionesController : ControllerBase
{
    private readonly IEleccionRepository _repo;
    public EleccionesController(IEleccionRepository repo) => _repo = repo;

    // GET /api/elecciones
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EleccionDto>>> GetAll()
        => Ok(await _repo.GetAllAsync());

    // GET /api/elecciones/{id}/listas
    [HttpGet("{id}/listas")]
    public async Task<ActionResult<IEnumerable<ListaElectoralDto>>> GetListas(string id)
        => Ok(await _repo.GetListasAsync(id));

    // GET /api/elecciones/{id}/candidatos
    [HttpGet("{id}/candidatos")]
    public async Task<ActionResult<IEnumerable<CandidatoDto>>> GetCandidatos(string id)
        => Ok(await _repo.GetCandidatosAsync(id));
}
