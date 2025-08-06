namespace EvolApp.Shared.Models;

public class ListaElectoral
{
    public string Id { get; set; }
    public string Nombre { get; set; }         // Ej: "Frente de Todos"
    public string EleccionId { get; set; }     // FK a Eleccion.Id
    public List<Candidato> Candidatos { get; set; } = new();
}