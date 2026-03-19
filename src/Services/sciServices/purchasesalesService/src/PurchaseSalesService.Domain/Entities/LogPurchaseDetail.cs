namespace PurchaseSalesService.Domain.Entities;

/// <summary>Maps to LOG_PURCHASE_DETAILS table.</summary>
public sealed class LogPurchaseDetail
{
    public long SerialNumber { get; init; }
    public long TrackingNumber { get; init; }
    public long TransactionNumber { get; init; }
    public long PurposeCode { get; init; }
    public long StageCode { get; init; }
    public long? OracleMerchandise { get; init; }
    public string? SupplierCode { get; init; }
    public long? TonNumLoaded { get; init; }
    public long? TonNumUnloaded { get; init; }
    public string UserId { get; init; } = null!;
    public long UserNumber { get; init; }
    public DateTime UpdatedAt { get; init; }
    public char? CancelFlag { get; init; }
    public string ModifiedBy { get; init; } = null!;
    public long ModifiedByNumber { get; init; }
    public DateTime ModifiedAt { get; init; }
}
