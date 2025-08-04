using EvolApp.Pages;
namespace EvolApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            // Registrar rutas para navegación con Shell
            Routing.RegisterRoute(nameof(EmpresasPage), typeof(EmpresasPage));
            Routing.RegisterRoute(nameof(AgregarEmpresaPage), typeof(AgregarEmpresaPage));
            Routing.RegisterRoute(nameof(WebViewPage), typeof(WebViewPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Volver al login
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
