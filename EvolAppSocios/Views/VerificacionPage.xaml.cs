using EvolAppSocios.ViewModels;

namespace EvolAppSocios.Views;

public partial class VerificacionPage : ContentPage
{
    public VerificacionPage(VerificacionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}