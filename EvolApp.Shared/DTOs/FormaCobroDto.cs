namespace EvolApp.Shared.DTOs
{
    public class FormaCobroDto
    {
        public string  IdFormaCobro { get; set; } = string.Empty;
        public string FormaCobro { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public int IdProceso { get; set; }
        public int IdProcesoProcesamiento { get; set; }
        public string CodigoFormaCobro { get; set; } = string.Empty;
    }
}