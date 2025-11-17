using Dapper;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;

namespace EvolApp.API.Repositories.Interfaces;

public interface IPrestamoRepository
{
    Task<IEnumerable<PrestamoDto>> ObtenerPorDocumento(string documentoOCuit);
    Task<PrestamoDetalleDto?> ObtenerPrestamoPorId(string idPrestamo);
    Task<PrestamoDetalleDto?> ObtenerPrestamoPorDocumento(string documentoOCuit);
    Task<IEnumerable<PrestamosPlanesDto>> ObtenerPlanesPorFormaCobro(string formaCobro);
    Task<dynamic> AltaEvolPrestamos(string json);
    Task<dynamic> ConsultaEvolPrestamos(string cuit);
}
