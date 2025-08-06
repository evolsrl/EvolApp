using CommunityToolkit.Maui;
using EvolAppSocios.Views;
using EvolAppSocios.Services;
using EvolAppSocios.ViewModels;
using EvolAppSocios;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        // 🔌 DI
        builder.Services.AddSingleton<AfiliadoApiService>();
        builder.Services.AddSingleton<RegistrarCuentaViewModel>();
        builder.Services.AddSingleton<RegistrarCuentaPage>();
        builder.Services.AddSingleton<VerificacionViewModel>();
        builder.Services.AddSingleton<VerificacionPage>();
        builder.Services.AddSingleton<CuentaAfiliadoViewModel>();
        builder.Services.AddSingleton<CuentaAfiliadoPage>();

        return builder.Build();
    }
}