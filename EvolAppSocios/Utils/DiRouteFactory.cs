using System;
using Microsoft.Extensions.DependencyInjection; // GetRequiredService
using Microsoft.Maui.Controls;
using EvolAppSocios.Utils;

public class DiRouteFactory<TPage> : RouteFactory where TPage : Page
{
    // Para escenarios donde Shell no pasa IServiceProvider
    public override Element GetOrCreate()
        => ServiceHelper.GetRequiredService<TPage>();

    // Preferido: Shell te pasa el IServiceProvider del app
    public override Element GetOrCreate(IServiceProvider services)
        => services.GetRequiredService<TPage>();
}
