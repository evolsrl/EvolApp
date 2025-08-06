using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolAppSocios.Services;
using EvolAppSocios.Views;

namespace EvolAppSocios.ViewModels;

[QueryProperty(nameof(Documento), "Documento")]
public partial class VerificacionViewModel : ObservableObject
{
    private readonly AfiliadoApiService _afiliadoApi;

    public VerificacionViewModel(AfiliadoApiService afiliadoApi)
    {
        _afiliadoApi = afiliadoApi;
    }

    [ObservableProperty]
    private string documento;

    [ObservableProperty]
    private string codigo;

    [RelayCommand]
    public async Task Verificar()
    {
        if (string.IsNullOrWhiteSpace(Codigo))
        {
            await Shell.Current.DisplayAlert("Error", "Debe ingresar el código", "OK");
            return;
        }

        var esValido = await _afiliadoApi.VerificarCodigo(Documento, Codigo);

        if (esValido)
        {
            Preferences.Set("CuentaVerificada", true);
            Preferences.Set("DocumentoVerificado", Documento);

            await Shell.Current.GoToAsync(nameof(CuentaAfiliadoPage), new Dictionary<string, object>
            {
                { "Documento", Documento }
            });
            //await Shell.Current.GoToAsync("//EstadoElectoralPage"); // Ruta protegida, próxima pantalla
        }
        else
        {
            await Shell.Current.DisplayAlert("Código incorrecto", "Verifique e intente nuevamente.", "OK");
        }
    }
}
