using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using EvolApp.Shared.Models;
using EvolAppSocios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

[ApiController]
[Route("api/afiliados")]
public class AfiliadosController : ControllerBase
{
    private readonly IAfiliadoRepository _repo;
    public AfiliadosController(IAfiliadoRepository repo) => _repo = repo;
    // GET /api/afiliados/{dni}
    [HttpGet("{documentoOCuit}")]
    public async Task<ActionResult<AfiliadoDto>> ObtenerPorDocumento(string documentoOCuit)
    {
        var afi = await _repo.ObtenerPorDocumento(documentoOCuit);
        return Ok(afi);
    }
    // POST /api/afiliados/{dni}/enviar-codigo
    [HttpPost("{dni}/enviar-codigo")]
    public async Task<IActionResult> EnviarCodigo(string dni)
    {
        await _repo.EnviarCodigo(dni);
        return Ok();
    }
    // POST /api/afiliados/{dni}/verificar
    [HttpPost("{dni}/verificar")]
    public async Task<ActionResult<ResultadoDTO>> VerificarCodigo(string dni, [FromBody] JsonElement body)
    {
        if (!body.TryGetProperty("codigo", out var c)) return BadRequest();
        var ok = await _repo.VerificarCodigo(dni, c.GetString()!);
        return Ok(ok);
    }
    //// POST /api/afiliados/alta
    //[Authorize]
    //[HttpPost("alta")]
    //public async Task<ActionResult> AltaAfiliado([FromBody] JsonElement body)
    //{
    //    if (!body.TryGetProperty("IdPrestamoSimulacion", out var idPrestamoSimulacion)) return BadRequest("Falta IdPrestamoSimulacion.");
    //    if (!body.TryGetProperty("IP", out var ip)) return BadRequest("Falta IP.");
    //    if (!body.TryGetProperty("ApellidoNombre", out var apellidoNombre)) return BadRequest("Falta ApellidoNombre.");
    //    if (!body.TryGetProperty("CorreoElectronico", out var correoElectronico)) return BadRequest("Falta CorreoElectronico.");
    //    if (!body.TryGetProperty("Celular", out var celular)) return BadRequest("Falta Celular.");
    //    if (!body.TryGetProperty("RangoHorario", out var rangoHorario)) return BadRequest("Falta RangoHorario.");
    //    if (!body.TryGetProperty("Observacion", out var observacion)) return BadRequest("Falta Observacion.");

    //    var dbparams = new DynamicParameters();

    //    dbparams.Add("IdPrestamoSimulacion", idPrestamoSimulacion.GetString(), DbType.String);
    //    dbparams.Add("IP", ip.GetString(), DbType.String);
    //    dbparams.Add("ApellidoNombre", apellidoNombre.GetString(), DbType.String);
    //    dbparams.Add("CorreoElectronico", correoElectronico.GetString(), DbType.String);
    //    dbparams.Add("Celular", celular.GetString(), DbType.String);
    //    dbparams.Add("RangoHorario", rangoHorario.GetString(), DbType.String);
    //    dbparams.Add("Observacion", observacion.GetString(), DbType.String);

    //    var result = await Task.FromResult(_dapper.Update<Prestamo>("[WordPressPrestamosSimulacionesActualizar]", dbparams, commandType: CommandType.StoredProcedure));

    //    if (result != null)
    //    {
    //        return Ok(result);
    //    }
    //    return BadRequest("Hubo un error al actualizar el préstamo.");
    //}

    //[Authorize]
    //[HttpPost("altaafiliado/")]
    //public async Task<ActionResult> AltaAfiliado([FromBody] JsonElement data)
    //{
    //    var dbparams = new DynamicParameters();
    //    dbparams.Add("Nombre", data.Nombre, DbType.String);
    //    dbparams.Add("Apellido", data.Apellido, DbType.String);
    //    dbparams.Add("TipoDocumento", data.IdTipoDocumento, DbType.String);
    //    dbparams.Add("NumeroDocumento", data.NumeroDocumento, DbType.String);
    //    dbparams.Add("NumeroSocio", data.NumeroSocio, DbType.String);
    //    dbparams.Add("NumeroLegajo", data.MatriculaIAF, DbType.String);
    //    dbparams.Add("Sexo", data.DescripcionSexo, DbType.String);
    //    dbparams.Add("FechaNacimiento", data.FechaNacimiento, DbType.String);
    //    dbparams.Add("FechaIngreso", data.FechaIngreso, DbType.String);
    //    dbparams.Add("EstadoCivil", data.DescripcionEstadoCivil, DbType.String);
    //    dbparams.Add("Categoria", data.DescripcionCategoria, DbType.String);
    //    dbparams.Add("Estado", data.IdEstado, DbType.String);
    //    dbparams.Add("Filial", data.IdFilial, DbType.String);
    //    dbparams.Add("CUIL", data.CUIL, DbType.String);
    //    dbparams.Add("CorreoElectronico", data.CorreoElectronico, DbType.String);
    //    dbparams.Add("Calle", data.Calle, DbType.String);
    //    dbparams.Add("Numero", data.Numero, DbType.String);
    //    dbparams.Add("Piso", data.Piso, DbType.String);
    //    dbparams.Add("Departamento", data.Departamento, DbType.String);
    //    dbparams.Add("CodigoPostal", data.CodigoPostal, DbType.String);
    //    dbparams.Add("Provincia", data.DescripcionProvincia, DbType.String);
    //    dbparams.Add("FormaCobro", data.IdFormaCobro, DbType.String);
    //    dbparams.Add("Dependencia", data.CodigoZonaGrupo, DbType.String);
    //    dbparams.Add("NumeroCelular", data.Celular, DbType.String);

    //    // Ejecutar el procedimiento almacenado y obtener el resultado (ID del afiliado o error)
    //    var result = await Task.FromResult(_dapper.Get<object>("[ApiAfiBeneficiosAltaAfiliado]", dbparams, commandType: CommandType.StoredProcedure));

    //    if (result is int idAfiliado && idAfiliado > 0)
    //    {
    //        // Si el resultado es un ID válido (mayor que 0), se devuelve el ID del afiliado
    //        return Ok(new { Success = true, IdAfiliado = idAfiliado });
    //    }
    //    else if (result is string errorMessage)
    //    {
    //        // Si el resultado es un mensaje de error (asumido como string), se devuelve ese mensaje
    //        return BadRequest(new { Success = false, ErrorMessage = errorMessage });
    //    }
    //    else
    //    {
    //        // Si no se recibe ni un ID ni un mensaje de error esperado, se devuelve un error genérico
    //        return BadRequest(new { Success = false, ErrorMessage = "Ocurrió un error desconocido." });
    //    }
    //}




}
