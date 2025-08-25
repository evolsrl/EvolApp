using EvolAppSocios.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Utils;
using EvolAppSocios.Views;

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

    [RelayCommand]
    public async Task IrAlPadron()
    {
        var session = ServiceHelper.GetRequiredService<SessionService>();
        var dni = string.IsNullOrWhiteSpace(Documento) ? session.Documento : Documento;
        await NavExtensions.GoToAsync(Shell.Current, AppRoute.Verificacion, "Documento", dni);
    }

    //[RelayCommand]
    //public async Task IrAlPadron()
    //{
    //    var session = EvolAppSocios.Utils.ServiceHelper.GetRequiredService<SessionService>();
    //    var dni = string.IsNullOrWhiteSpace(Documento) ? session.Documento : Documento;

    //    if (string.IsNullOrWhiteSpace(dni))
    //    {
    //        await Shell.Current.DisplayAlert("Atención", "No se pudo determinar el documento.", "OK");
    //        return;
    //    }
    //    await Shell.Current.GoToAsync(AppRoute.Verificacion);
    //}

}
