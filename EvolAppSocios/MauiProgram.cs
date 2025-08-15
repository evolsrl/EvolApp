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

        // ViewModels
        builder.Services.AddTransient<RegistrarCuentaViewModel>();
        builder.Services.AddTransient<CuentaAfiliadoViewModel>();
        builder.Services.AddTransient<VerificacionViewModel>();
        builder.Services.AddTransient<VotacionViewModel>();

        // Pages
        builder.Services.AddTransient<RegistrarCuentaPage>();
        builder.Services.AddTransient<CuentaAfiliadoPage>();
        builder.Services.AddTransient<VerificacionPage>();
        builder.Services.AddTransient<VotacionPage>();

        // Shell por DI
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<SessionService>();

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
