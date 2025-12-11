using Dapper;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;
using System.Data;
using System.Threading.Tasks;

namespace EvolApp.API.Repositories.Interfaces;

public interface IPrestamoRepository
{
    Task<IEnumerable<PrestamoDto>> ObtenerPorDocumento(string documentoOCuit);
    Task<IEnumerable<CuotaProximaDto>> ObtenerCuotasProximasVencerPorDocumento(string documentoOCuit);
    Task<PrestamoDetalleDto?> ObtenerPrestamoPorId(string idPrestamo);
    Task<PrestamoDetalleDto?> ObtenerPrestamoPorDocumento(string documentoOCuit);
    Task<IEnumerable<PrestamosPlanesDto>> ObtenerPlanesPrestamoSimulacion();
    Task<IEnumerable<PrestamosPlanesDto>> ObtenerPrestamosCantidadCuotasSimulacion(string idPlan);
    Task<List<CuotaDto>> ArmarCuponera(
            int? idTipoOperacion,
            int idPrestamoPlan,
            int? idPrestamoPlanTasa,
            int cantidadCuotas,
            string importeSolicitado
        );
    Task<IEnumerable<PrestamosPlanesDto>> ObtenerPlanesPorFormaCobro(string formaCobro);
    Task<dynamic> AltaEvolPrestamos(string json);
    Task<IEnumerable<dynamic>> ConsultaEvolPrestamos(string cuit);
}
