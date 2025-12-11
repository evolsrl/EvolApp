using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<NavItemDto>> GetOpcionesAsync(string documento);
    }
}
