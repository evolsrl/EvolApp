using Microsoft.Extensions.DependencyInjection;

namespace EvolAppSocios.Utils;

public static class ServiceHelper
{
    public static IServiceProvider Services { get; private set; } = default!;
    public static void ConfigureServices(IServiceProvider services) => Services = services;
    public static T GetRequiredService<T>() where T : notnull => Services.GetRequiredService<T>();
}
