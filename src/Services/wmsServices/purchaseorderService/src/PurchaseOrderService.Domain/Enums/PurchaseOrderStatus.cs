namespace PurchaseOrderService.Domain.Enums;

public enum PurchaseOrderStatus
{
    Draft = 0,
    Confirmed = 1,
    Receiving = 2,
    Completed = 3,
    Cancelled = 4
}

public static class PurchaseOrderStatusExtensions
{
    public static string ToDbString(this PurchaseOrderStatus status) => status switch
    {
        PurchaseOrderStatus.Draft => "DRAFT",
        PurchaseOrderStatus.Confirmed => "CONFIRMED",
        PurchaseOrderStatus.Receiving => "RECEIVING",
        PurchaseOrderStatus.Completed => "COMPLETED",
        PurchaseOrderStatus.Cancelled => "CANCELLED",
        _ => throw new ArgumentOutOfRangeException(nameof(status))
    };

    public static PurchaseOrderStatus FromDbString(string value) => value switch
    {
        "DRAFT" => PurchaseOrderStatus.Draft,
        "CONFIRMED" => PurchaseOrderStatus.Confirmed,
        "RECEIVING" => PurchaseOrderStatus.Receiving,
        "COMPLETED" => PurchaseOrderStatus.Completed,
        "CANCELLED" => PurchaseOrderStatus.Cancelled,
        _ => throw new ArgumentOutOfRangeException(nameof(value))
    };
}
