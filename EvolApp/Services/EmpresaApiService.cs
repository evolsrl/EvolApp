using EvolApp.Shared.Models;
using EvolApp.Utils;
using System.Text;
using System.Text.Json;

namespace EvolApp.Services;

public static class EmpresaApiService
{
    public static async Task<Empresa?> ValidarYObtenerEmpresa(string usuario, string password, string cuit)
    {
        string clave = CryptoUtils.EncriptarHash(password);
        var requestBody = new
        {
            Usuario = usuario,
            Clave = clave,
            Cuit = cuit
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var http = new HttpClient();
        http.DefaultRequestHeaders.Add("X-API-KEY", "evolapp-supersecreta-123");

        var response = await http.PostAsync("https://erp.evol.com.ar/evolappapi/api/empresas/login", content);

        if (!response.IsSuccessStatusCode)
            return null;

        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<Empresa>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    //public static async Task<Empresa?> ValidarYObtenerEmpresa(string usuario, string password, string cuit)
    //{
    //    // Simulamos un pequeño retraso de red
    //    await Task.Delay(1000);

    //    // Validación falsa
    //    if (usuario == "admin" && password == "1234" && cuit == "20270503714")
    //    {
    //        return new Empresa
    //        {
    //            Nombre = "SAN ANTONIO PREMOLDEADOS SRL",
    //            Url = "https://erp.evol.com.ar/sap",
    //            Cuit = cuit
    //        };
    //    }

    //    // Si no coincide con los valores simulados, devuelve null (fallo de autenticación)
    //    return null;
    //}
}
