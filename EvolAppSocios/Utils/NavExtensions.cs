using Microsoft.Maui.Controls;

namespace EvolAppSocios.Utils;

public static class NavExtensions
{
    // GoTo con enum y parámetros (usa ToString, NO nameof)
    public static Task GoToAsync(this Shell shell, AppRoute route, IDictionary<string, object>? query = null)
        => query is null
           ? shell.GoToAsync(route.ToString())
           : shell.GoToAsync(route.ToString(), new ShellNavigationQueryParameters(query));

    // Overload simple clave/valor
    public static Task GoToAsync(this Shell shell, AppRoute route, string key, object value)
        => shell.GoToAsync(route, new Dictionary<string, object> { [key] = value });

    // Push creando la página desde DI
    public static async Task PushAsync<TPage>(this Shell shell, Action<TPage>? init = null)
        where TPage : Page
    {
        var page = ServiceHelper.GetRequiredService<TPage>();
        init?.Invoke(page);
        await shell.Navigation.PushAsync(page);
    }
}
