using EvolAppSocios.Utils;
using EvolAppSocios.ViewModels;

namespace EvolAppSocios.Views;

public partial class VotacionPage : ContentPage
{
    private readonly VotacionViewModel _viewModel;

    public VotacionPage(VotacionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //await _viewModel.CargarAfiliado();
    }
    
}
