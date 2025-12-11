using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/menu")]
public class MenuController : ControllerBase
{
    private readonly IMenuRepository _repo;
    public MenuController(IMenuRepository repo) => _repo = repo;

    // GET /api/menu/{documento}
    [HttpGet("{documento}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<List<NavItemDto>>> GetOpcionesAsync([FromRoute] string documento)
    {
        if (string.IsNullOrWhiteSpace(documento))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var result = await _repo.GetOpcionesAsync(documento);

        return Ok(result);
    }
}
