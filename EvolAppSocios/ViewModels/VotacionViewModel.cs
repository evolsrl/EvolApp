using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;
using EvolAppSocios.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace EvolAppSocios.ViewModels
{
    public partial class VotacionViewModel : ObservableObject
    {
        private readonly VotacionApiService _api;

        [ObservableProperty]
        private EleccionDto? eleccion;                      // DTO en vez de Eleccion

        [ObservableProperty]
        private ObservableCollection<ListaElectoralDto> listas = new(); // DTO

        [ObservableProperty]
        private ListaElectoralDto? listaSeleccionada;       // DTO

        public VotacionViewModel(VotacionApiService api)
            => _api = api;

        [RelayCommand]
        public async Task CargarDatosAsync()
        {
            var elecciones = await _api.ObtenerEleccionesAsync();
            Eleccion = elecciones.FirstOrDefault();
            if (Eleccion is null) return;

            var listasApi = await _api.ObtenerListasPorEleccionAsync(Eleccion.Id);
            Listas = new ObservableCollection<ListaElectoralDto>(listasApi);
        }

        [RelayCommand]
        public async Task VotarAsync()
        {
            if (ListaSeleccionada is null)
            {
                await Shell.Current.DisplayAlert("Error", "Debe seleccionar una lista", "OK");
                return;
            }

            await Shell.Current.DisplayAlert("Voto registrado",
                $"Votaste por: {ListaSeleccionada.Nombre}", "OK");
        }
    }
}
