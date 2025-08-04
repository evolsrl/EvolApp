namespace EvolApp.API.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Cuit { get; set; }
        public string Usuario { get; set; }
        public string Clave { get; set; }

        public string Nombre { get; set; }
        public string Url { get; set; }
    }
}
