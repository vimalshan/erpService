namespace WMTransactional.Application.DTOs;

public record ReceivingDto
{
    public int ReceivingId { get; init; }
    public string ReceivingNumber { get; init; } = null!;
    public int PoId { get; init; }
    public DateTime ReceivedDate { get; init; }
    public string Status { get; init; } = null!;
    public string? Notes { get; init; }
    public string? CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public List<ReceivingLineDto> Lines { get; init; } = [];
}

public record ReceivingLineDto
{
    public int ReceivingLineId { get; init; }
    public int ReceivingId { get; init; }
    public int PoLineId { get; init; }
    public int ProductId { get; init; }
    public int BinId { get; init; }
    public decimal QuantityReceived { get; init; }
    public string? LotNumber { get; init; }
    public DateTime? ExpiryDate { get; init; }
    public string? Notes { get; init; }
}
