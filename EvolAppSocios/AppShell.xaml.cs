using EvolAppSocios.Services;
using EvolAppSocios.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EvolAppSocios
{
    public partial class AppShell : Shell
    {
        private static readonly HashSet<string> _rutasRegistradas =
            new(StringComparer.OrdinalIgnoreCase);

        public AppShell(RegistrarCuentaPage startPage) // ✅ inyectamos la página inicial
        {
            InitializeComponent();

            // Página inicial armada con DI
            var start = new ShellContent
            {
                Title = "Registrar Cuenta",
                Content = startPage,
                Route = nameof(RegistrarCuentaPage)
            };
            Items.Add(start);

            // Rutas que vas a usar por navegación
            Routing.RegisterRoute(nameof(CuentaAfiliadoPage), typeof(CuentaAfiliadoPage));
            Routing.RegisterRoute(nameof(VerificacionPage), typeof(VerificacionPage));
            Routing.RegisterRoute(nameof(VotacionPage), typeof(VotacionPage));

            // Si la cuenta ya estaba verificada, redirigimos cuando el Shell está listo
            var verificada = Preferences.Get("CuentaVerificada", false);
            var doc = Preferences.Get("DocumentoVerificado", string.Empty);

            if (verificada && !string.IsNullOrWhiteSpace(doc))
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                    await GoToAsync($"{nameof(CuentaAfiliadoPage)}?Documento={Uri.EscapeDataString(doc)}"));
            }
        }

        private static void RegistrarRutaSiHaceFalta(string route, Type pageType)
        {
            if (_rutasRegistradas.Contains(route)) return;
            Routing.RegisterRoute(route, pageType);
            _rutasRegistradas.Add(route);
        }

        public async Task CargarMenuDinamicoAsync(string dni)
        {
            var servicioMenu = new MenuApiService();
            var opciones = await servicioMenu.ObtenerOpciones(dni);

            EvolAppSocios.Utils.MenuMapper.MapearTipos(opciones);

            // Construir primero los nuevos items
            var nuevos = new List<ShellContent>();
            foreach (var op in opciones)
            {
                if (op.PageType is null) continue;

                // Registrar la ruta una sola vez
                RegistrarRutaSiHaceFalta(op.Ruta, op.PageType);

                nuevos.Add(new ShellContent
                {
                    Title = op.Nombre,
                    ContentTemplate = new DataTemplate(op.PageType),
                    Route = op.Ruta
                });
            }

            // Reemplazo atómico
            Items.Clear();
            foreach (var sc in nuevos)
                Items.Add(sc);

            if (CurrentItem is null && Items.Count > 0)
                CurrentItem = Items[0];
        }
    }

}
