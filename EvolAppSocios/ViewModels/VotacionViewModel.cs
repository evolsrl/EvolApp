using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Services;
using System.Collections.ObjectModel;
namespace EvolAppSocios.ViewModels;

public partial class VotacionViewModel : ObservableObject
{
    private readonly VotacionApiService _api;
    private readonly SessionService _session;

    public VotacionViewModel(VotacionApiService api, SessionService session)
    {
        _api = api;
        _session = session;

        Listas = new ObservableCollection<ListaElectoralDto>();
        Candidatos = new ObservableCollection<CandidatoDto>();
        TextoVacio = "Cargando listas...";
    }

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string textoVacio = "Sin datos";

    public ObservableCollection<ListaElectoralDto> Listas { get; }
    public ObservableCollection<CandidatoDto> Candidatos { get; }

    [ObservableProperty]
    private ListaElectoralDto? listaSeleccionada;

    partial void OnListaSeleccionadaChanged(ListaElectoralDto? value)
    {
        // Cargar candidatos de la lista elegida
        _ = CargarCandidatosAsync(value);
        OnPropertyChanged(nameof(PuedeConfirmar));
        ConfirmarVotoCommand.NotifyCanExecuteChanged();
    }

    public bool PuedeConfirmar => !IsBusy && ListaSeleccionada is not null;

    [RelayCommand]
    public async Task CargarDatosAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Listas.Clear();

            // Documento para contexto (si lo necesitás)
            var dni = _session.Documento;

            var eleccion = await _api.ObtenerEleccionesAsync();

            var listas = await _api.ObtenerListasPorEleccionAsync(eleccion[0].Id);
            //foreach (var l in listas)
            //{
            //    Listas.Add(new ListaVm
            //    {
            //        Id = l.Id,
            //        Nombre = l.Nombre,
            //        Descripcion = l.Descripcion,
            //        LogoUrl = l.LogoUrl
            //    });
            //}

            TextoVacio = Listas.Count == 0 ? "No hay listas disponibles" : "";
        }
        catch (Exception ex)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(ex);
#endif
            TextoVacio = "Error al cargar listas.";
            await Shell.Current.DisplayAlert("Error", "No se pudieron cargar las listas.", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(PuedeConfirmar));
            ConfirmarVotoCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private async Task RefrescarAsync()
    {
        await CargarDatosAsync();
    }

    private async Task CargarCandidatosAsync(ListaElectoralDto? lista)
    {
        Candidatos.Clear();
        if (lista is null) return;

        try
        {
            IsBusy = true;
            var candidatos = await _api.ObtenerCandidatosPorListaAsync(lista.Id);
        }
        catch (Exception ex)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(ex);
#endif
            await Shell.Current.DisplayAlert("Error", "No se pudieron cargar los candidatos.", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(PuedeConfirmar));
            ConfirmarVotoCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand(CanExecute = nameof(PuedeConfirmar))]
    private async Task ConfirmarVotoAsync()
    {
        if (ListaSeleccionada is null) return;

        try
        {
            IsBusy = true;

            var ok = true;// await _api.RegistrarVotoAsync(_session.Documento, ListaSeleccionada.Id);
            if (ok)
            {
                await Shell.Current.DisplayAlert("Voto registrado",
                    $"Votaste por: {ListaSeleccionada.Nombre}", "OK");

                // si querés volver a la cuenta:
                // await Shell.Current.GoToAsync(AppRoute.CuentaAfiliado, "Documento", _session.Documento);
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo registrar el voto.", "OK");
            }
        }
        catch (Exception ex)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(ex);
#endif
            await Shell.Current.DisplayAlert("Error", "Ocurrió un problema al registrar el voto.", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(PuedeConfirmar));
            ConfirmarVotoCommand.NotifyCanExecuteChanged();
        }
    }
}

public sealed class ListaVm
{
    public string Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
}

public sealed class CandidatoVm
{
    public string Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Cargo { get; set; } = "";
    public string? FotoUrl { get; set; }
}
