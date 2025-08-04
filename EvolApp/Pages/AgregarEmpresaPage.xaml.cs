using EvolApp.Models;
using EvolApp.Services;
using EvolApp.Utils;

namespace EvolApp.Pages;

public partial class AgregarEmpresaPage : ContentPage
{
    public AgregarEmpresaPage()
    {
        InitializeComponent();
    }

    private async void OnAgregarClicked(object sender, EventArgs e)
    {
        var usuario = usuarioEntry.Text?.Trim();
        var password = passwordEntry.Text;
        var cuit = cuitEntry.Text?.Trim();

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(cuit))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        try
        {
            var empresa = await EmpresaApiService.ValidarYObtenerEmpresa(usuario, password, cuit);

            if (empresa is null)
            {
                await DisplayAlert("Error", "Credenciales inválidas o CUIT no encontrado.", "OK");
                return;
            }
            empresa.Usuario= CryptoUtils.EncriptarTexto(usuario);
            empresa.Clave= CryptoUtils.EncriptarTexto(password);
            empresa.Cuit = cuit;
            EmpresaStorageService.GuardarEmpresa(empresa);
            await DisplayAlert("Éxito", $"Empresa '{empresa.Nombre}' agregada.", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo conectar: {ex.Message}", "OK");
        }
    }
}