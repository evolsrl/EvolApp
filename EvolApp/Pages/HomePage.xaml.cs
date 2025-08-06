using EvolApp.Services;
using EvolApp.Shared.Models;

namespace EvolApp.Pages;

public partial class HomePage : ContentPage
{
    private List<Empresa> empresas = new();
    public HomePage()
    {
        InitializeComponent();
        Loaded += HomePage_Loaded!;
    }

    private async void HomePage_Loaded(object sender, EventArgs e)
    {
        await Task.Delay(500); // pequeño delay para mostrar cargando
        var empresas = EmpresaStorageService.ObtenerEmpresas();

        if (empresas.Count == 0)
        {
            // No hay empresas → Ir a EmpresasPage para agregar
            await Navigation.PushAsync(new EmpresasPage());
        }
        //else if (empresas.Count == 1)
        //{
        //    // Solo una empresa → abrir su URL directamente
        //    var empresa = empresas.First();
        //    await Navigation.PushAsync(new WebViewPage(empresa));
        //}
        else
        {
            CargarEmpresas();
        }
    }
    private void CargarEmpresas()
    {
        empresas = EmpresaStorageService.ObtenerEmpresas();
        empresasView.ItemsSource = empresas;
    }
    private async void OnEmpresaBotonClicked(object sender, EventArgs e)
    {
        if (sender is Button boton && boton.CommandParameter is Empresa empresa)
        {
            await Navigation.PushAsync(new WebViewPage(empresa));
        }
    }
    private async void OnConfigurarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EmpresasPage());
    }
}
