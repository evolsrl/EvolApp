using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/menu")]
[ApiExplorerSettings(IgnoreApi = true)]
public class MenuController : ControllerBase
{
    private readonly IMenuRepository _repo;
    public MenuController(IMenuRepository repo) => _repo = repo;

    // GET /api/menu/{dni}
    [HttpGet("{dni}")]
    public async Task<ActionResult<IEnumerable<OpcionMenuDto>>> Get(string dni)
        => Ok(await _repo.GetOpcionesAsync(dni));
}
