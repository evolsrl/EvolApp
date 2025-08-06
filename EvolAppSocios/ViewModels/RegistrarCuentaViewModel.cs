using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;           // ← DTO compartido
using EvolAppSocios.Services;
using Microsoft.Maui.Controls;       // Para Shell

namespace EvolAppSocios.ViewModels
{
    public partial class RegistrarCuentaViewModel : ObservableObject
    {
        private readonly AfiliadoApiService _afiliadoApi;

        [ObservableProperty]
        private string documento = string.Empty;

        [ObservableProperty]
        private AfiliadoDto? afiliado;       // ← DTO en vez de EvolApp.Shared.Models.Afiliado

        [ObservableProperty]
        private bool afiliadoEncontrado;

        public RegistrarCuentaViewModel(AfiliadoApiService afiliadoApi)
            => _afiliadoApi = afiliadoApi;

        [RelayCommand]
        public async Task BuscarAfiliado()
        {
            if (string.IsNullOrWhiteSpace(Documento))
            {
                await Shell.Current.DisplayAlert("Error", "Debes ingresar un documento", "OK");
                return;
            }

            // Ahora tu servicio devuelve AfiliadoDto
            Afiliado = await _afiliadoApi.ObtenerAfiliado(Documento);
            AfiliadoEncontrado = Afiliado != null;

            if (AfiliadoEncontrado)
            {
                await _afiliadoApi.EnviarCodigo(Documento);

                // Navegación usando query param (asegúrate de registrar ruta "verificacion")
                await Shell.Current.GoToAsync($"verificacion?Documento={Documento}");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Afiliado no encontrado.", "OK");
            }
        }
    }
}
