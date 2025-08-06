namespace EvolApp.Shared.DTOs
{
    public class EleccionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int AnioDesde { get; set; }
        public int AnioHasta { get; set; }
    }
}