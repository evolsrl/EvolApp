using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IEleccionRepository
    {
        Task<IEnumerable<EleccionDto>> GetAllAsync();
        Task<IEnumerable<ListaElectoralDto>> GetListasAsync(string eleccionId);
        Task<IEnumerable<CandidatoDto>> GetCandidatosAsync(string listaId);
    }
}
