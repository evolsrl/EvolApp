using System.Buffers.Text;

namespace EvolApp.Shared.DTOs
{
    public class NavItemDto
    {
        public string IdMenu { get; set; } = string.Empty;
        public string Menu { get; set; } = string.Empty;
        public string href { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
    }
}