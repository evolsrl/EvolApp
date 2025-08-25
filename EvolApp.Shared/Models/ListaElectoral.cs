namespace EvolApp.Shared.Models;

public class ListaElectoral
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string EleccionId { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;
    public List<Candidato> Candidatos { get; set; } = new();
}