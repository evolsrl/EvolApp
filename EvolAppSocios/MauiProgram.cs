using CommunityToolkit.Maui;
using EvolAppSocios;
using EvolAppSocios.Http;
using EvolAppSocios.Mappers;
using EvolAppSocios.Services;
using EvolAppSocios.ViewModels;
using EvolAppSocios.Views;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(AfiliadoProfile));

        // Providers y seguridad
        builder.Services.AddSingleton<IApiSettingsProvider, ApiSettingsProvider>();
        builder.Services.AddSingleton<ISecurityContext, SecurityContext>();

        // Handlers
        builder.Services.AddTransient<DiagnosticsHandler>();     // ✅ registrar
        builder.Services.AddTransient<SecurityHeaderHandler>();

        // Typed HttpClients (¡¡sin AddSingleton del servicio!!)
        builder.Services
            .AddHttpClient<AfiliadoApiService>()
            .AddHttpMessageHandler<DiagnosticsHandler>()
            .AddHttpMessageHandler<SecurityHeaderHandler>();

        builder.Services
            .AddHttpClient<VotacionApiService>()
            .AddHttpMessageHandler<DiagnosticsHandler>()
            .AddHttpMessageHandler<SecurityHeaderHandler>();

        // ViewModels (podés dejarlos Singleton si te funciona así)
        builder.Services.AddSingleton<RegistrarCuentaViewModel>();
        builder.Services.AddSingleton<CuentaAfiliadoViewModel>();
        builder.Services.AddSingleton<VerificacionViewModel>();
        builder.Services.AddSingleton<VotacionViewModel>();

        // Vistas
        builder.Services.AddSingleton<RegistrarCuentaPage>();
        builder.Services.AddSingleton<CuentaAfiliadoPage>();
        builder.Services.AddSingleton<VotacionPage>();

        // Shell por DI
        builder.Services.AddSingleton<AppShell>();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("=== EXCEPTION ===");
            System.Diagnostics.Debug.WriteLine(e.Exception.Message.ToString()); // incluye stack trace con el assembly culpable
            System.Diagnostics.Debug.WriteLine("=======================================");
#endif
        };

        var app = builder.Build();
        // Guardar el ServiceProvider para el helper
        EvolAppSocios.Utils.ServiceHelper.Services = app.Services;
        return app;
    }
}
