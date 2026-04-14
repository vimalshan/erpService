namespace WMTransactional.Domain.ValueObjects;

public sealed record ShipmentStatus
{
    private static readonly HashSet<string> ValidStatuses =
    [
        "OPEN", "SHIPPED", "CANCELLED"
    ];

    public static readonly ShipmentStatus Open = new("OPEN");
    public static readonly ShipmentStatus Shipped = new("SHIPPED");
    public static readonly ShipmentStatus Cancelled = new("CANCELLED");

    public string Value { get; }

    private ShipmentStatus(string value)
    {
        if (!ValidStatuses.Contains(value))
            throw new ArgumentException($"Invalid shipment status: {value}");
        Value = value;
    }

    public static ShipmentStatus From(string value) => new(value.ToUpperInvariant());

    public override string ToString() => Value;
}
