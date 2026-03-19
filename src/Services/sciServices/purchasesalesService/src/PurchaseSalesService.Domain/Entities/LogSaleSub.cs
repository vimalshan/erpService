namespace PurchaseSalesService.Domain.Entities;

/// <summary>Maps to LOG_SALE_SUB table.</summary>
public sealed class LogSaleSub
{
    public long ReferenceNumber { get; init; }
    public long SerialNumber { get; init; }
    public string ProductCode { get; init; } = null!;
    public decimal? ProductQuantity { get; init; }
    public string ProductGrade { get; init; } = null!;
    public string? UserComment { get; init; }
    public long? CheckbookInvoice { get; init; }
    public char? CancelFlag { get; init; }
    public string ModifiedBy { get; init; } = null!;
    public long ModifiedByNumber { get; init; }
    public DateTime ModifiedAt { get; init; }
}
