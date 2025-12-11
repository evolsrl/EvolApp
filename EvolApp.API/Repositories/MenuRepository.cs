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

        public async Task<List<NavItemDto>> GetOpcionesAsync(string documento)
        {
            var navbar = await _db.QueryAsync<NavItemDto>(
                "EvolAppApiGetMenuOpciones",
                new
                {
                    NumeroDocumento = documento
                },
                commandType: CommandType.StoredProcedure
            );

            return navbar.ToList();
        }
    }
}