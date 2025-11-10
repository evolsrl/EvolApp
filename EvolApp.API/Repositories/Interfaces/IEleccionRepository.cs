using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IEleccionRepository
    {
        Task<IEnumerable<EleccionDto>> GetAllAsync();
        Task<IEnumerable<ListaElectoralDto>> GetListasAsync(string eleccionId);
        Task<IEnumerable<CandidatoDto>> GetCandidatosAsync(string listaId);
        Task<ResultadoDTO> GetValidarVotoAsync(string listaId, string documento);
        Task<ResultadoDTO> GetVotarAsync(string listaId, string documento);
    }
}
