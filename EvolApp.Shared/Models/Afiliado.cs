using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EvolApp.Shared.Models
{
    public class Afiliado
    {
        public string Documento { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string FechaNacimiento { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        public string CorreoOculto =>
            Regex.Replace(Correo, @"(?<=.).(?=.*@)", "*")
                 .Replace("@", "@...");
    }
}
