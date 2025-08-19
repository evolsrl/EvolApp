// Services/SessionService.cs
using EvolApp.Shared.Models;

namespace EvolAppSocios.Services;

public class SessionService
{
    // Estado mínimo que te interesa
    public bool EstaVerificada { get; private set; }
    public string Documento { get; private set; } = string.Empty;
    public Afiliado? AfiliadoActual { get; private set; }

    // Persistencia opcional (Preferences)
    const string K_VER = "CuentaVerificada";
    const string K_DOC = "DocumentoVerificado";

    public void EstablecerSesion(Afiliado afiliado)
    {
        AfiliadoActual = afiliado;
        Documento = afiliado.Documento;
        EstaVerificada = true;

        // Persistir (opcional)
        Preferences.Set(K_VER, true);
        Preferences.Set(K_DOC, Documento);
    }

    public void CargarDesdePreferencias()
    {
        EstaVerificada = Preferences.Get(K_VER, false);
        Documento = Preferences.Get(K_DOC, string.Empty);
        // Si querés, podés lazy-load del backend el Afiliado con ese Documento
        // AfiliadoActual = ...
    }

    public void CerrarSesion()
    {
        EstaVerificada = false;
        Documento = string.Empty;
        AfiliadoActual = null;

        Preferences.Remove(K_VER);
        Preferences.Remove(K_DOC);
    }
}
