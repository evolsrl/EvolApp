using EvolApp.Models;

namespace EvolApp.Pages;

public partial class WebViewPage : ContentPage
{
    public WebViewPage(Empresa empresa)
    {
        InitializeComponent();

        Title = empresa.Nombre;

        // Mostrar el spinner al inicio
        loadingIndicator.IsVisible = true;
        loadingIndicator.IsRunning = true;
        string parametros = Uri.EscapeDataString(string.Concat(empresa.Usuario, "|", empresa.Clave));
        string url = string.Concat(empresa.Url, "IngresoApp.aspx?login=", parametros);
        empresaWebView.Source = new UrlWebViewSource
        {
            Url = url
        };
    }

    private void OnNavigating(object sender, WebNavigatingEventArgs e)
    {
        loadingIndicator.IsVisible = true;
        loadingIndicator.IsRunning = true;
    }

    private void OnNavigated(object sender, WebNavigatedEventArgs e)
    {
        loadingIndicator.IsRunning = false;
        loadingIndicator.IsVisible = false;
    }
}
