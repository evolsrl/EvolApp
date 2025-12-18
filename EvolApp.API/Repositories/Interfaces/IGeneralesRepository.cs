using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IGeneralesRepository
    {
        Task<IEnumerable<FormaCobroDto>> GetAllAsync();
        Task<IEnumerable<EmpresaDto>> GetEmpresas();
        Task<ResultadoDTO> GetEndpoint(string cuit);
    }
}
