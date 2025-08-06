using EvolApp.Shared.Models;
using System.Text.Json;

namespace EvolApp.Services;

public static class EmpresaStorageService
{
    private const string Key = "empresas";

    public static List<Empresa> ObtenerEmpresas()
    {
        var json = Preferences.Get(Key, "");
        if (string.IsNullOrWhiteSpace(json))
            return new List<Empresa>();

        var empresas = JsonSerializer.Deserialize<List<Empresa>>(json) ?? new();
        return empresas.OrderBy(e => e.Nombre).ToList();
    }

    public static void GuardarEmpresa(Empresa nueva)
    {
        var empresas = ObtenerEmpresas();

        if (!empresas.Any(e => e.Cuit == nueva.Cuit))
        {
            empresas.Add(nueva);
            var json = JsonSerializer.Serialize(empresas);
            Preferences.Set(Key, json);
        }
    }
    public static void EliminarEmpresa(Empresa nueva)
    {
        var empresas = ObtenerEmpresas();
        var nuevasEmpresas = empresas.Where(e => e.Cuit != nueva.Cuit).ToList();
        var json = JsonSerializer.Serialize(nuevasEmpresas);
        Preferences.Set(Key, json);
    }

    public static void Limpiar()
    {
        Preferences.Remove(Key);
    }
}