using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<AfiliadoDto?> GetByDocumentoAsync(string documento);
        Task SendCodeAsync(string documento);
        Task<ResultadoDTO> VerifyCodeAsync(string documento, string codigo);
    }
}
