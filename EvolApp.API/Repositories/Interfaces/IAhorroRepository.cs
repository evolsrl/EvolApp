using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IAhorroRepository
    {
        Task<IEnumerable<dynamic>> ProgramaPuntosCajasAhorrosPuntos();
        Task<IEnumerable<dynamic>> ProgramaPuntosNumerosSorteos(DateTime fecha);
        Task<IEnumerable<dynamic>> ProgramaPuntosCodigosConceptos();
        Task<IEnumerable<dynamic>> ProgramaPuntosCodigosParticipaciones();
    }
}
