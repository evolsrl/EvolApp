using System.Text.Json.Serialization;
namespace EvolApp.Shared.Models;
public class OpcionMenu
{
    public string Clave { get; set; }      // "votacion"
    public string Nombre { get; set; }     // "Votación"
    public string Pagina { get; set; }     // "VotacionPage" (viene de la API)

    [JsonIgnore]
    public Type? PageType { get; set; }    // Se completa del lado del cliente

    public string Ruta => Clave;
}
