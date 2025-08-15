using EvolAppSocios.ViewModels;
using EvolAppSocios.Utils;

namespace EvolAppSocios.Views;

public partial class CuentaAfiliadoPage : ContentPage
{
    private readonly CuentaAfiliadoViewModel _viewModel;

    public CuentaAfiliadoPage(CuentaAfiliadoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.CargarAfiliado();

        if (!MenuSessionTracker.MenuCargado && !string.IsNullOrWhiteSpace(_viewModel.Documento))
        {
            await (Shell.Current as AppShell)!.CargarMenuDinamicoAsync(_viewModel.Documento);
            MenuSessionTracker.MenuCargado = true;
        }
    }
}