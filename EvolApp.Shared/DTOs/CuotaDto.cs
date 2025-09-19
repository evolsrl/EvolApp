namespace EvolApp.Shared.DTOs;

public record CuotaDto
{
    public int Numero { get; init; }
    public DateTime? Vencimiento { get; init; }
    public decimal? Importe { get; init; }
    public bool? Pagado { get; init; }
    public string? Estado { get; init; }
    public DateTime? FechaPago { get; init; }
}
