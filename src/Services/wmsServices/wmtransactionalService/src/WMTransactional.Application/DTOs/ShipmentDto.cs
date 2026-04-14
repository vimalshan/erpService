namespace WMTransactional.Application.DTOs;

public record ShipmentDto
{
    public int ShipmentId { get; init; }
    public string ShipmentNumber { get; init; } = null!;
    public int SoId { get; init; }
    public DateTime ShippedDate { get; init; }
    public string Status { get; init; } = null!;
    public string? TrackingNumber { get; init; }
    public string? Carrier { get; init; }
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public List<ShipmentLineDto> Lines { get; init; } = [];
}

public record ShipmentLineDto
{
    public int ShipmentLineId { get; init; }
    public int ShipmentId { get; init; }
    public int SoLineId { get; init; }
    public int ProductId { get; init; }
    public int BinId { get; init; }
    public decimal QuantityShipped { get; init; }
    public string? LotNumber { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? Notes { get; init; }
}
