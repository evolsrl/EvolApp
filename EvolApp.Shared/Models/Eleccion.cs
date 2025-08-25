// Models/Eleccion.cs
namespace EvolApp.Shared.Models;

public class Eleccion
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int AnioDesde { get; set; } 
    public int AnioHasta { get; set; }          // Ej: 2025
    public List<ListaElectoral> Listas { get; set; } = new();
}
