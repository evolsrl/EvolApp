using System.Text.Json.Serialization;
namespace EvolApp.Shared.Models;
public class OpcionMenu
{
    public string Clave { get; set; } = string.Empty;    // "votacion"
    public string Nombre { get; set; } = string.Empty;     // "Votación"
    public string Pagina { get; set; } = string.Empty;    // "VotacionPage" (viene de la API)

    [JsonIgnore]
    public Type? PageType { get; set; }    // Se completa del lado del cliente

    public string Ruta => Clave;
}
