namespace WMTransactional.Infrastructure.Messaging;

public record PurchaseOrderCreatedMessage
{
    public string PoNumber { get; init; } = null!;
    public int SupplierId { get; init; }
    public DateTime OccurredOn { get; init; }
}

public record PurchaseOrderStatusChangedMessage
{
    public string PoNumber { get; init; } = null!;
    public int SupplierId { get; init; }
    public string NewStatus { get; init; } = null!;
    public DateTime OccurredOn { get; init; }
}

public record SalesOrderCreatedMessage
{
    public string SoNumber { get; init; } = null!;
    public int CustomerId { get; init; }
    public DateTime OccurredOn { get; init; }
}

public record SalesOrderStatusChangedMessage
{
    public string SoNumber { get; init; } = null!;
    public int CustomerId { get; init; }
    public string NewStatus { get; init; } = null!;
    public DateTime OccurredOn { get; init; }
}

public record ShipmentShippedMessage
{
    public string ShipmentNumber { get; init; } = null!;
    public int SoId { get; init; }
    public DateTime OccurredOn { get; init; }
}

public record ReceivingCompletedMessage
{
    public string ReceivingNumber { get; init; } = null!;
    public int PoId { get; init; }
    public DateTime OccurredOn { get; init; }
}
