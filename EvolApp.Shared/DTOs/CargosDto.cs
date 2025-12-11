namespace EvolApp.Shared.DTOs;

public record CargosDto
{
    public int Posicion { get; init; }
    public int IdCuentaCorriente { get; init; }
    public int IdAfiliado { get; init; }
    public int Periodo { get; init; }
    public DateTime? FechaMovimiento { get; init; }
    public string? Concepto { get; init; }
    public decimal? Importe { get; init; }
    public decimal? ImporteCobrado { get; init; }
}