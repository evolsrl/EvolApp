using EvolAppSocios.Services;
using EvolAppSocios.Utils;
using EvolAppSocios.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EvolAppSocios
{
    public partial class AppShell : Shell
    {
        private static readonly HashSet<string> _rutasRegistradas =
            new(StringComparer.OrdinalIgnoreCase);
        private readonly SessionService _session;


        public AppShell(RegistrarCuentaPage startPage, SessionService session)
        {
            InitializeComponent();
            _session = session;

            Items.Add(new ShellContent
            {
                Title = "Registrar Cuenta",
                Content = startPage,
                Route = nameof(AppRoute.RegistrarCuenta)
            });

            RouteMap.RegisterAll();

            // Restaurar sesión (si había)
            _session.CargarDesdePreferencias();

            Loaded += async (_, __) =>
            {
                if (_session.EstaVerificada && !string.IsNullOrWhiteSpace(_session.Documento))
                {
                    await Shell.Current.GoToAsync(AppRoute.CuentaAfiliado, "Documento", _session.Documento);
                }
            };
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
