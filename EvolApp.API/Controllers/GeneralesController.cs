using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

[ApiController]
[Route("api/generales")]
[ApiExplorerSettings(IgnoreApi = true)]
public class GeneralesController : ControllerBase
{
    private readonly IGeneralesRepository _repo;
    public GeneralesController(IGeneralesRepository repo) => _repo = repo;

    // GET /api/generales/formas-cobros
    [HttpGet("formas-cobros")]
    public async Task<ActionResult<IEnumerable<FormaCobroDto>>> GetAll()
        => Ok(await _repo.GetAllAsync());
}
