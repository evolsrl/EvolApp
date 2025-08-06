using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IVotacionRepository
    {
        Task VoteAsync(VotacionRequest dto);
    }
}
