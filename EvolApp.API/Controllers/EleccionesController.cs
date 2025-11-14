using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

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

    // GET /api/elecciones/{id}/validar-voto?documento=13587241
    [HttpGet("{id}/validar-voto")]
    public async Task<ActionResult<ResultadoDTO>> GetValidarVoto(string id, [FromQuery] string documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            return BadRequest("Falta documento");

        var ok = await _repo.GetValidarVotoAsync(id, documento);
        return Ok(ok);
    }

    // GET /api/votar
    [HttpGet("votar")]
    public async Task<ActionResult<ResultadoDTO>> GetVotar([FromQuery] string id, [FromQuery] string documento)
        => Ok(await _repo.GetVotarAsync(id,documento));
}
