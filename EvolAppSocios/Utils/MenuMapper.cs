using EvolApp.Shared.DTOs;
using EvolApp.Shared.Models;
using EvolAppSocios.Views;

namespace EvolAppSocios.Utils
{
    public static class MenuMapper
    {
        private static readonly Dictionary<string, Type> PaginasDisponibles = new()
    {
        { "CuentaAfiliadoPage", typeof(CuentaAfiliadoPage) },
        { "VotacionPage", typeof(VotacionPage) }
    };

        public static void MapearTipos(List<OpcionMenu> opciones)
        {
            foreach (var opcion in opciones)
            {
                if (PaginasDisponibles.TryGetValue(opcion.Pagina, out var tipo))
                    opcion.PageType = tipo;
            }
        }
    }
}
