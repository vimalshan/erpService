namespace WMTransactional.Domain.ValueObjects;

public sealed record OrderStatus
{
    private static readonly HashSet<string> ValidStatuses =
    [
        "DRAFT", "CONFIRMED", "RECEIVING", "COMPLETED", "CANCELLED",
        "PICKING", "SHIPPING"
    ];

    public static readonly OrderStatus Draft = new("DRAFT");
    public static readonly OrderStatus Confirmed = new("CONFIRMED");
    public static readonly OrderStatus Receiving = new("RECEIVING");
    public static readonly OrderStatus Completed = new("COMPLETED");
    public static readonly OrderStatus Cancelled = new("CANCELLED");
    public static readonly OrderStatus Picking = new("PICKING");
    public static readonly OrderStatus Shipping = new("SHIPPING");

    public string Value { get; }

    private OrderStatus(string value)
    {
        if (!ValidStatuses.Contains(value))
            throw new ArgumentException($"Invalid order status: {value}");
        Value = value;
    }

    public static OrderStatus From(string value) => new(value.ToUpperInvariant());

    public override string ToString() => Value;
}
