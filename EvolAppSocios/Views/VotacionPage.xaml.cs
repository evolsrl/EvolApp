// Views/VotacionPage.xaml.cs
using EvolAppSocios.ViewModels;

namespace EvolAppSocios.Views;

public partial class VotacionPage : ContentPage
{
    private readonly VotacionViewModel _vm;

    public VotacionPage(VotacionViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CargarDatosAsync();
    }
}
