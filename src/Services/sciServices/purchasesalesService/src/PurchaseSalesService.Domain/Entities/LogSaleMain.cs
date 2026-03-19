namespace PurchaseSalesService.Domain.Entities;

/// <summary>Maps to LOG_SALE_MAIN table.</summary>
public sealed class LogSaleMain
{
    public long SerialNumber { get; init; }
    public long TrackingNumber { get; init; }
    public long TransactionNumber { get; init; }
    public long PurposeCode { get; init; }
    public long StageCode { get; init; }
    public string? IsoNumber { get; init; }
    public DateTime? IsoDate { get; init; }
    public string? ProductDescription { get; init; }
    public string UserId { get; init; } = null!;
    public long UserNumber { get; init; }
    public DateTime UpdatedAt { get; init; }
    public char? CancelFlag { get; init; }
    public string ModifiedBy { get; init; } = null!;
    public long ModifiedByNumber { get; init; }
    public DateTime ModifiedAt { get; init; }
}
