// Models/Eleccion.cs
namespace EvolApp.Shared.Models;

public class Eleccion
{
    public string Id { get; set; }
    public string Nombre { get; set; }          // Ej: "Elecciones Generales"
    public int AnioDesde { get; set; }          // Ej: 2025
    public int AnioHasta { get; set; }          // Ej: 2025
    public List<ListaElectoral> Listas { get; set; } = new();
}
