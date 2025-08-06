using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<AfiliadoDto?> GetByDocumentoAsync(string documento);
        Task SendCodeAsync(string documento);
        Task<bool> VerifyCodeAsync(string documento, string codigo);
    }
}
