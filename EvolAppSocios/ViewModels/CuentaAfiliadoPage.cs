using EvolAppSocios.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;

namespace EvolAppSocios.ViewModels;

[QueryProperty(nameof(Documento), "Documento")]
public partial class CuentaAfiliadoViewModel : ObservableObject
{
    private readonly AfiliadoApiService _afiliadoApi;

    public CuentaAfiliadoViewModel(AfiliadoApiService afiliadoApi)
    {
        _afiliadoApi = afiliadoApi;
    }

    [ObservableProperty]
    private string documento;

    [ObservableProperty]
    private AfiliadoDto? afiliado;

    public async Task CargarAfiliado()
    {
        Afiliado = await _afiliadoApi.ObtenerAfiliado(Documento);
    }

    //[RelayCommand]
    //public async Task IrAlPadron()
    //{
    //    await Shell.Current.GoToAsync(nameof(EstadoElectoralPage), new Dictionary<string, object>
    //    {
    //        { "Documento", Documento }
    //    });
    //}
}
