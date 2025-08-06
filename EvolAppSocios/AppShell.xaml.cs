using EvolAppSocios.Services;
using EvolAppSocios.Utils;
using EvolAppSocios.Views;

namespace EvolAppSocios
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas para navegación interna
            Routing.RegisterRoute(nameof(VerificacionPage), typeof(VerificacionPage));
            Routing.RegisterRoute(nameof(CuentaAfiliadoPage), typeof(CuentaAfiliadoPage));

        }
        public async Task CargarMenuDinamicoAsync(string dni)
        {
            var servicioMenu = new MenuApiService();
            var opciones = await servicioMenu.ObtenerOpciones(dni);

            Items.Clear();

            foreach (var opcion in opciones)
            {
                if (opcion.PageType != null)
                {
                    Items.Add(new ShellContent
                    {
                        Title = opcion.Nombre,
                        ContentTemplate = new DataTemplate(opcion.PageType),
                        Route = opcion.Ruta
                    });

                    Routing.RegisterRoute(opcion.Ruta, opcion.PageType);
                }
            }
        }
    }
}
