using EvolApp.Shared.DTOs;
using EvolAppSocios.Models;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<AfiliadoDto?> ObtenerPorDocumento(string documentoOCuit);
        Task EnviarCodigo(string documento);
        Task<ResultadoDTO> VerificarCodigo(string documento, string codigo);
        Task<ResultadoDTO> AltaEvolSocios(string json);
        Task<ResultadoDTO> ActualizarEvolSocios(string json);
        Task<ResultadoDTO> ExisteEvolSocios(string json);
        Task<ResultadoDTO> ConsultaEvolSocios(string json);
        //Task<AfiliadoDto?> AltaSocio(string documentoOCuit);
    }
}
