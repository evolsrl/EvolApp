using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using System.Data;

public class VotacionRepository : IVotacionRepository
{
    private readonly IDbConnection _db;
    public VotacionRepository(IDbConnection db) => _db = db;

    public async Task VoteAsync(VotacionRequest dto)
    {
        await _db.ExecuteAsync(
            "EvolAppApiEleccionesVotosInsertar",
            new
            {
                AfiliadoDocumento = dto.DocumentoAfiliado,
                ListaId = dto.ListaId,
                DispositivoInfo = dto.DispositivoInfo
            },
            commandType: CommandType.StoredProcedure);
    }
}