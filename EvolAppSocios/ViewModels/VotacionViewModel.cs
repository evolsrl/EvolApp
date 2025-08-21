using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;
using EvolApp.Shared.Models;
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

        Listas = new ObservableCollection<ListaElectoral>();
        Candidatos = new ObservableCollection<Candidato>();
        TextoVacio = "Cargando listas...";
    }

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string textoVacio = "Sin datos";

    public ObservableCollection<ListaElectoral> Listas { get; }
    public ObservableCollection<Candidato> Candidatos { get; }

    [ObservableProperty]
    private ListaElectoral? listaSeleccionada;

    partial void OnListaSeleccionadaChanged(ListaElectoral? value)
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
            TextoVacio = "Cargando listas...";

            // (1) Elecciones disponibles
            var elecciones = await _api.ObtenerEleccionesAsync();
            if (elecciones is null || elecciones.Count == 0)
            {
                TextoVacio = "No hay elecciones disponibles.";
                return;
            }

            var eleccion = elecciones[0]; // si querés otra, elegila acá

            // (2) Listas de esa elección
            var listas = await _api.ObtenerListasPorEleccionAsync(eleccion.Id);
            if (listas is null || listas.Count == 0)
            {
                TextoVacio = "No hay listas para esta elección.";
                return;
            }

            foreach (var l in listas)
            {
                Listas.Add(new ListaElectoral
                {
                    Id = l.Id,
                    Nombre = l.Nombre
                });
            }

            TextoVacio = "";
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

    private async Task CargarCandidatosAsync(ListaElectoral? lista)
    {
        Candidatos.Clear();
        if (lista is null) return;

        try
        {
            IsBusy = true;
            var candidatos = await _api.ObtenerCandidatosPorListaAsync(lista.Id);
            if (candidatos is null || candidatos.Count == 0) return;

            foreach (var c in candidatos)
            {
                Candidatos.Add(new Candidato
                {
                    Documento = c.Documento,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Cargo = c.Cargo
                });
            }
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