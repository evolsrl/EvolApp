using Dapper;
using EvolApp.Shared.DTOs;
using System.Data;

namespace EvolApp.API.Repositories.Interfaces;

public interface IPrestamoRepository
{
    Task<IEnumerable<PrestamoDto>> ObtenerPorDocumento(string documentoOCuit);
    Task<PrestamoDetalleDto?> ObtenerPrestamoPorId(string idPrestamo);
    Task<PrestamoDetalleDto?> ObtenerPrestamoPorDocumento(string documentoOCuit);
}
