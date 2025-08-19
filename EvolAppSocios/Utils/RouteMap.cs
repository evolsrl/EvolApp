using Microsoft.Maui.Controls;
using EvolAppSocios.Views;

namespace EvolAppSocios.Utils;

public static class RouteMap
{
    public static void RegisterAll()
    {
        Routing.RegisterRoute(AppRoute.RegistrarCuenta.ToString(), new DiRouteFactory<RegistrarCuentaPage>());
        Routing.RegisterRoute(AppRoute.CuentaAfiliado.ToString(), new DiRouteFactory<CuentaAfiliadoPage>());
        Routing.RegisterRoute(AppRoute.Verificacion.ToString(), new DiRouteFactory<VerificacionPage>());
        Routing.RegisterRoute(AppRoute.Votacion.ToString(), new DiRouteFactory<VotacionPage>());
    }
}