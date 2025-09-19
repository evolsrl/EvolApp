using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<AfiliadoDto?> ObtenerPorDocumento(string documentoOCuit);
        Task EnviarCodigo(string documento);
        Task<ResultadoDTO> VerificarCodigo(string documento, string codigo);
        //Task<AfiliadoDto?> AltaSocio(string documentoOCuit);
    }
}
