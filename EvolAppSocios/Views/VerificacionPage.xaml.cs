using EvolAppSocios.Utils;
using EvolAppSocios.ViewModels;

namespace EvolAppSocios.Views;

public partial class VerificacionPage : ContentPage, IQueryAttributable
{
    private readonly VerificacionViewModel _vm;

    // ✅ ctor sin parámetros requerido por Shell
    public VerificacionPage()
    {
        InitializeComponent();

        // Resolver el VM desde DI
        _vm = ServiceHelper.GetRequiredService<VerificacionViewModel>();
        BindingContext = _vm;
    }

    // Recibir parámetros de navegación y pasarlos al VM
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Documento", out var val))
            _vm.Documento = val?.ToString() ?? string.Empty;
    }
}