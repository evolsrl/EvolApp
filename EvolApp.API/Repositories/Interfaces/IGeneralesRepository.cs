using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IGeneralesRepository
    {
        Task<IEnumerable<FormaCobroDto>> GetAllAsync();
        Task<ResultadoDTO> GetEndpoint(string cuit);
    }
}
