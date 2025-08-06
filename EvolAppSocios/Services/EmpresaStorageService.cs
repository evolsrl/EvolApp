using EvolApp.Shared.Models;
using System.Text.Json;

namespace EvolAppSocios.Services;

public static class EmpresaStorageService
{
    private const string Key = "empresas";

    public static Empresa ObtenerEmpresas()
    {
        var json = Preferences.Get(Key, "");
        if (string.IsNullOrWhiteSpace(json))
            return new Empresa();

        var empresas = JsonSerializer.Deserialize<Empresa>(json) ?? new();
        return empresas;
    }

    public static void GuardarEmpresa(Empresa nueva)
    {
        var json = JsonSerializer.Serialize(nueva);
        Preferences.Set(Key, json);
    }
    public static void EliminarEmpresa(Empresa nueva)
    {
        Limpiar();
    }

    public static void Limpiar()
    {
        Preferences.Remove(Key);
    }
}