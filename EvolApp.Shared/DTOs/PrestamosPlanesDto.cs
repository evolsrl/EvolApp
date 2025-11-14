namespace EvolApp.Shared.DTOs
{
    public class PrestamosPlanesDto
    {
        public string IdPrestamoPlan { get; set; } = string.Empty;
        public decimal PorcentajeGasto { get; set; }
        public decimal ImporteGasto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuarioEvento { get; set; }
        public DateTime FechaInicioVigencia { get; set; }
        public DateTime FechaFinVigencia { get; set; }
        public decimal PorcentajeCapitalSocial { get; set; }
        public int IdMoneda { get; set; }
        public int IdTipoUnidad { get; set; }
        public decimal PorcentajeSeguroCuota { get; set; }
        public decimal RedondeoCuota { get; set; }
    }
}