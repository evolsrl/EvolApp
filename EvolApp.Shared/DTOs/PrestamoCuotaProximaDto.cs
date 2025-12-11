namespace EvolApp.Shared.DTOs
{
    public record CuotaProximaDto
    {
        public long? PrestamoId { get; init; }
        public string? PrestamoNumero { get; init; }
        public int? Numero { get; init; }
        public DateTime? Vencimiento { get; init; }
        public decimal? Importe { get; init; }
        public bool? Pagado { get; init; }
        public string? Estado { get; init; }
    }
}
