namespace EvolAppSocios
{
    public partial class App : Application
    {
        // MAUI crea App con DI, así que podés inyectar
        public App(AppShell shell)
        {
            InitializeComponent();
            MainPage = shell; // ✅ No necesitamos App.Current.Services
        }
    }
}
