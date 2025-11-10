namespace EvolApp.Shared.DTOs
{
    public class AfiliadoDto
    {
        public int IdAfiliado { get; set; } = 0;
        public string Documento { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string FechaNacimiento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Exito { get; set; } = Convert.ToBoolean(0);
        public bool TieneCuenta { get; set; } = Convert.ToBoolean(0);
        public string Mensaje { get; set; } = string.Empty;
    }
}