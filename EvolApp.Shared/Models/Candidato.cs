namespace EvolApp.Shared.Models;

public class Candidato
{
    public string Documento { get; set; } = string.Empty;      // DNI o pasaporte
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
}