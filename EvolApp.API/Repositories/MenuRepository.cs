using Dapper;
using EvolApp.API.Repositories.Interfaces;
using EvolApp.Shared.DTOs;
using System.Data;
namespace EvolApp.API.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IDbConnection _db;
        public MenuRepository(IDbConnection db) => _db = db;

        public async Task<IEnumerable<OpcionMenuDto>> GetOpcionesAsync(string documento)
        {
            return await _db.QueryAsync<OpcionMenuDto>(
                "EvolAppApiGetMenuOpciones",
                new { Documento = documento },
                commandType: CommandType.StoredProcedure);
        }
    }
}