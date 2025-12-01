using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace EvolApp.API.Controllers;

[ApiController]
[Route("api")]
public class PrestamosController : ControllerBase
{
    private readonly IPrestamoRepository _repo;
    public PrestamosController(IPrestamoRepository repo)
    {
        _repo = repo;
    }
    // GET /api/prestamos/documento/{documentoOCuit}
    [HttpGet("prestamos/documento/{documentoOCuit}")]
    public async Task<ActionResult<IEnumerable<PrestamoDto>>> ObtenerPorDocumento([FromRoute] string documentoOCuit)
    {
        if (string.IsNullOrWhiteSpace(documentoOCuit))
            return BadRequest("Documento requerido.");

        var data = await _repo.ObtenerPorDocumento(documentoOCuit);
        return Ok(data);
    }
    // GET /api/prestamosDetalles/id/{id}
    [HttpGet("prestamosDetalles/id/{id}")]
    public async Task<ActionResult<PrestamoDetalleDto>> ObtenerPrestamoPorId([FromRoute] string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest("Id de préstamo requerido.");

        var data = await _repo.ObtenerPrestamoPorId(id);
        if (data is null) return NotFound();

        return Ok(data);
    }
    // GET /api/prestamosDetalles/documento/{documentoOCuit}
    [HttpGet("prestamosDetalles/documento/{documentoOCuit}")]
    public async Task<ActionResult<PrestamoDetalleDto>> ObtenerPrestamoPorDocumento([FromRoute] string documentoOCuit)
    {
        if (string.IsNullOrWhiteSpace(documentoOCuit))
            return BadRequest("DocumentoOCuit de préstamo requerido.");

        var data = await _repo.ObtenerPrestamoPorDocumento(documentoOCuit);
        if (data is null) return NotFound();

        return Ok(data);
    }
    // GET /api/prestamosDetalles/documento/{documentoOCuit}
    [HttpGet("prestamosDetalles/planes/{formaCobro}")]
    public async Task<ActionResult<IEnumerable<PrestamosPlanesDto>>> ObtenerPlanesPorFormaCobro(string formaCobro)
    {
        var planes = await _repo.ObtenerPlanesPorFormaCobro(formaCobro);
        return Ok(planes ?? Enumerable.Empty<PrestamosPlanesDto>());
    }

    /// <summary>
    /// Agregar préstamo a la cuenta de un afiliado.
    /// </summary>
    /// <remarks>
    /// <code>
    /// {
    ///    "Nombre": "HUGO TEST API",
    ///    "Apellido": "REYNOSO",
    ///    "TipoDocumento": "DNI",
    ///    "NumeroDocumento": 44320332,
    ///    "CUIL": 20443203320,
    ///    "NumeroSocio": "101172/00",
    ///    "NumeroLegajo": 1435,
    ///    "Sexo": "Masculino",
    ///    "FechaNacimiento": "1973-05-31T00:00:00",
    ///    "FechaIngreso": "2009-03-20T00:00:00",
    ///    "TipoPersona": "Fisica",
    ///    "AfiliadoTipo": "Titular",
    ///    "EstadoCivil": "Casado/a",
    ///    "CorreoElectronico": "NO TIENE",
    ///    "Categoria": "Activo",
    ///    "Estado": "Normal",
    ///    "Dependencia": "",
    ///    "Grado": "CAPITAN",
    ///    "GrupoSanguineo": "A +",
    ///    "CondicionFiscal": "Consumidor Final",
    ///    "CBU": null,
    ///    "Filial": "Filial Villa Ramallo",
    ///    "Detalle":"Este es un testing de detalle, lo hago largo para verificar longitud.",
    ///    "Domicilios": [
    ///        {
    ///            "IdDomicilio": 96352,
    ///            "IdDomicilioTipo": 1,
    ///            "DomicilioTipo": "Particular",
    ///            "Calle": "RAFAEL OBLIGADO 2",
    ///            "Numero": 19613,
    ///            "Piso": 4,
    ///            "Departamento": "03",
    ///            "IdCodigoPostal": 16862,
    ///            "IdProvincia": 2,
    ///            "Provincia": "Buenos Aires",
    ///            "Localidad": "RAMALLO",
    ///            "Predeterminado": true,
    ///            "IdEstado": 1,
    ///            "CodigoPostal": "2915"
    ///        }
    ///    ],
    ///    "Telefonos": [
    ///        {
    ///            "IdTelefono": 95546,
    ///            "IdTelefonoTipo": 1,
    ///            "Numero": 3407437164,
    ///            "Interno": 0,
    ///            "IdEstado": 1
    ///        },
    ///        {
    ///    "IdTelefono": 95547,
    ///            "IdTelefonoTipo": 1,
    ///            "Numero": 5493407467968,
    ///            "Interno": 0,
    ///            "IdEstado": 1
    ///        }
    ///    ],
    ///    "FormasCobros": [
    ///        {
    ///            "FormaCobro": "Caja",
    ///            "Predeterminado": false,
    ///            "IdEstado": 1,
    ///            "IdFormaCobroAfiliado": 63432
    ///        },
    ///        {
    ///    "FormaCobro": "FIPLASTO S.A. SINDIC",
    ///            "Predeterminado": true,
    ///            "IdEstado": 1,
    ///            "IdFormaCobroAfiliado": 63433
    ///        }
    ///    ]
    ///}
    ///</code>
    /// </remarks>
    /// <param name="json"></param>
    /// <returns></returns>
    [HttpPost("prestamos/agregar")]
    public async Task<IActionResult> Agregar([FromBody] JsonElement json)
    {
        var res = await _repo.AltaEvolPrestamos(json.GetRawText());
        return Ok(res);
    }

    /// <summary>
    /// Consulta de préstamos asociados a un CUIT.
    /// </summary>
    /// <remarks>
    /// Devuelve la lista de préstamos asociados al CUIT indicado, con el detalle de las cuotas y el estado e importe cobrado de cada una.
    /// </remarks>
    /// <param name="cuit"></param>
    /// <returns></returns>
    [HttpGet("prestamos/consultar/{cuit}")]
    public async Task<IActionResult> Consultar(string cuit)
    {
        var prestamos = await _repo.ConsultaEvolPrestamos(cuit);

        if (prestamos == null || !prestamos.Any())
            return NotFound(new { exito = false, mensaje = "No se encontraron préstamos para el CUIT indicado." });

        return Ok(prestamos);
    }
}