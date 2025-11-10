using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<AfiliadoDto?> ObtenerPorDocumento(string documentoOCuit);
        Task EnviarCodigo(string documento);
        Task<ResultadoDTO> VerificarCodigo(string documento, string codigo);
        Task<ResultadoDTO> RegistrarAfiliado(string documento, string username, string password);
        Task<AfiliadoDto?> LoguearAfiliado(string documento, string password);
        Task<IEnumerable<FormaCobroDto>> ObtenerFormasCobrosPorDocumento(string documentoOCuit);
    }
}
