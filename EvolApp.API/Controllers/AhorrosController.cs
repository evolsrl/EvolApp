using EvolApp.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EvolApp.API.Swagger;

namespace EvolApp.API.Controllers
{
    [ApiController]
    [Route("api/ahorros")]
    public class AhorrosController : ControllerBase
    {
        private readonly IAhorroRepository _repo;
        public AhorrosController(IAhorroRepository repo) => _repo = repo;

        [HttpGet("programapuntos/cajasahorrospuntos")]
        public async Task<IActionResult> ProgramaPuntosCajasAhorrosPuntos()
        {
            var res = await _repo.ProgramaPuntosCajasAhorrosPuntos();
            return Ok(res);
        }

        [HttpGet("programapuntos/numerossorteos/{fecha}")]
        public async Task<IActionResult> ProgramaPuntosNumerosSorteos(DateTime fecha)
        {
            var res = await _repo.ProgramaPuntosNumerosSorteos(fecha);
            return Ok(res);
        }

        [HttpGet("programapuntos/codigosconceptos")]
        public async Task<IActionResult> ProgramaPuntosCodigosConceptos()
        {
            var res = await _repo.ProgramaPuntosCodigosConceptos();
            return Ok(res);
        }

        [HttpGet("programapuntos/codigosparticipaciones")]
        public async Task<IActionResult> ProgramaPuntosCodigosParticipaciones()
        {
            var res = await _repo.ProgramaPuntosCodigosParticipaciones();
            return Ok(res);
        }

    }
}
