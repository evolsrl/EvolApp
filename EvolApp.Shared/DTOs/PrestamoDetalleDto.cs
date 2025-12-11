namespace EvolApp.Shared.DTOs;

public record PrestamoDetalleDto
{
    // Cabecera
    public long Id { get; init; }
    public string Numero { get; init; } = string.Empty;
    public DateTime? FechaAlta { get; init; }
    public decimal? Capital { get; init; }
    public string Cuotas { get; init; } = string.Empty;
    public string? Estado { get; init; }
    public decimal? ImporteCuota { get; init; }
    public decimal? Saldo { get; init; }
    
    // Cuponera
    public List<CuotaDto> Cuponera { get; init; } = new();
}
