
namespace EvolApp.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //var isAvailable = await CrossFingerprint.Current.IsAvailableAsync();

        //if (!isAvailable)
        //{
        //    await DisplayAlert("Seguridad no disponible",
        //        "Este dispositivo no tiene autenticación biométrica ni clave configurada.",
        //        "Cerrar");

        //    System.Diagnostics.Process.GetCurrentProcess().Kill();
        //    return;
        //}

        //var result = await CrossFingerprint.Current.AuthenticateAsync(
        //    new AuthenticationRequestConfiguration("Acceso seguro", "Autenticarse para ingresar"));

        //if (!result.Authenticated)
        //{
        //    await DisplayAlert("Acceso denegado", "No se pudo verificar la identidad", "OK");
        //    System.Diagnostics.Process.GetCurrentProcess().Kill();
        //    return;
        //}

        // Acceso exitoso → navegar a la pantalla principal
        await Navigation.PushAsync(new EmpresasPage());
    }
}