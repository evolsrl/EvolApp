using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/listas")]
public class ListasController : ControllerBase
{
    private readonly IEleccionRepository _repo;
    public ListasController(IEleccionRepository repo) => _repo = repo;

    // GET /api/listas/{listaId}/candidatos
    [HttpGet("{listaId}/candidatos")]
    public async Task<ActionResult<IEnumerable<CandidatoDto>>> GetCandidatos(string listaId)
        => Ok(await _repo.GetCandidatosAsync(listaId));
}
