
namespace EvolAppSocios.Http;

// Hacé esta clase PUBLIC para facilitar la deserialización
public sealed class RootConfig
{
    public ApiSettings? ApiSettings { get; set; }
}
