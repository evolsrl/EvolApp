using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.Models;
using EvolAppSocios.Services;
using EvolAppSocios.Utils;

public partial class VerificacionViewModel : ObservableObject
{
    private readonly SessionService _session;
    private readonly AfiliadoApiService _afiliadoApi;
    private readonly IMapper _mapper;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarCommand))]
    private string documento = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarCommand))]
    private string codigo = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ConfirmarCommand))]
    private bool isBusy;

    public VerificacionViewModel(SessionService session, AfiliadoApiService api, IMapper mapper)
    {
        _session = session;
        _afiliadoApi = api;
        _mapper = mapper;
    }

    [RelayCommand(CanExecute = nameof(PuedeConfirmar))]
    private async Task ConfirmarAsync()
    {
        try
        {
            IsBusy = true; // ✅ usar la propiedad, NO el campo

            var resultado = await _afiliadoApi.VerificarCodigo(Documento, Codigo); // ✅ propiedades
            if (resultado?.Exito == true)
            {
                var dto = await _afiliadoApi.ObtenerAfiliado(Documento);
                if (dto is null)
                {
                    await Shell.Current.DisplayAlert("Error", "No se pudo obtener el afiliado.", "OK");
                    return;
                }

                var afiliado = _mapper.Map<Afiliado>(dto);
                _session.EstablecerSesion(afiliado);

                await Shell.Current.GoToAsync(AppRoute.CuentaAfiliado, "Documento", Documento);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", resultado?.Mensaje ?? "Código inválido.", "OK");
            }
        }
        finally
        {
            IsBusy = false; // ✅ propiedad
        }
    }

    private bool PuedeConfirmar()
        => !IsBusy && !string.IsNullOrWhiteSpace(Documento) && !string.IsNullOrWhiteSpace(Codigo); // ✅ propiedades
}
