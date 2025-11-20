using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EvolApp.API.Models;

namespace EvolApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EmpresasController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmpresasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] EmpresaLoginRequest request)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();

                var command = new SqlCommand("EvolAppApiGrupoEmpresasSeleccionarPorCuit", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@Cuit", request.Cuit);
                command.Parameters.AddWithValue("@Usuario", request.Usuario);
                command.Parameters.AddWithValue("@Clave", request.Clave);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var response = new EmpresaLoginResponse
                    {
                        Nombre = reader["Nombre"]?.ToString(),
                        Url = reader["Url"]?.ToString()
                    };

                    return Ok(response);
                }

                return Unauthorized("CUIT o credenciales inválidas.");
            }
            catch (SqlException ex)
            {
                // Si viene de RAISERROR o THROW
                return BadRequest($"Error en base de datos: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
