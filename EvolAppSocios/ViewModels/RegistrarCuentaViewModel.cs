using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.Models;
using EvolAppSocios.Services;
using EvolAppSocios.Utils;
using EvolAppSocios.Views;

public partial class RegistrarCuentaViewModel : ObservableObject
{
    private readonly AfiliadoApiService _afiliadoApi;
    private readonly IMapper _mapper;

    [ObservableProperty] private string documento = string.Empty;
    [ObservableProperty] private Afiliado? afiliado;
    [ObservableProperty] private bool afiliadoEncontrado;

    public RegistrarCuentaViewModel(AfiliadoApiService afiliadoApi, IMapper mapper)
    {
        _afiliadoApi = afiliadoApi;
        _mapper = mapper;
    }

    [RelayCommand]
    public async Task BuscarAfiliado()
    {
        var dto = await _afiliadoApi.ObtenerAfiliado(Documento);
        afiliado = dto is not null ? _mapper.Map<Afiliado>(dto) : null;
        afiliadoEncontrado = afiliado is not null;

        if (AfiliadoEncontrado)
        {
            await _afiliadoApi.EnviarCodigo(Documento);
            await Shell.Current.GoToAsync(AppRoute.Verificacion, "Documento", Documento);
        }
    }
}
