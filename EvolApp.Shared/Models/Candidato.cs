namespace EvolApp.Shared.Models;

public class Candidato
{
    public string Documento { get; set; }      // DNI o pasaporte
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Cargo { get; set; }          // Ej: "Diputado Nacional"
}