namespace EvolApp.Shared.DTOs
{
    public class AfiliadoDto
    {
        public string Documento { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}