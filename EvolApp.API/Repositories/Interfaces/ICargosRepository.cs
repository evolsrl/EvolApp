using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface ICargosRepository
    {
        Task<IEnumerable<dynamic>> ObtenerCargosPendientes(string cuit);
        Task<IEnumerable<dynamic>> ObtenerCuentaCorrienteCargos(string cuit);
        Task<dynamic> ObtenerCuentaCorrienteCargosContratados(string cuit);
    }
}
