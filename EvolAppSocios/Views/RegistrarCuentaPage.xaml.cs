using EvolAppSocios.ViewModels;

namespace EvolAppSocios.Views;

public partial class RegistrarCuentaPage : ContentPage
{
    public RegistrarCuentaPage(RegistrarCuentaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}