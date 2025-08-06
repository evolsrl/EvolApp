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
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }

        public string CorreoOculto =>
            Regex.Replace(Correo, @"(?<=.).(?=.*@)", "*")
                 .Replace("@", "@...");
    }
}
