using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface ICargosRepository
    {
        Task<List<CargosDto>> ObtenerCargosPendientes(string cuit);
        Task<List<CargosDto>> ObtenerCuentaCorrienteCargos(string cuit);
    }
}
