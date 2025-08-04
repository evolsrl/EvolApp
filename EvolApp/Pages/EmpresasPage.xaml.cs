using EvolApp.Models;
using EvolApp.Services;

namespace EvolApp.Pages;

public partial class EmpresasPage : ContentPage
{
    private List<Empresa> empresas = new();

    public EmpresasPage()
    {
        InitializeComponent();
        CargarEmpresas();
    }

    private void CargarEmpresas()
    {
        empresas = EmpresaStorageService.ObtenerEmpresas();
        empresasView.ItemsSource = empresas;
    }

    private async void OnAgregarEmpresaClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AgregarEmpresaPage());
    }

    private async void OnEmpresaSeleccionada(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Empresa empresa)
        {
            await Navigation.PushAsync(new WebViewPage(empresa));
        }
    }
    private async void OnEliminarEmpresaClicked(object sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is Empresa empresa)
        {
            bool confirm = await DisplayAlert("Eliminar empresa", $"¿Deseás eliminar la empresa {empresa.Nombre}?", "Sí", "No");
            if (confirm)
            {
                EmpresaStorageService.EliminarEmpresa(empresa);
                CargarEmpresas();
            }
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarEmpresas(); // refrescar al volver
    }
}