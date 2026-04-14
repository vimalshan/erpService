namespace WMTransactional.Domain.ValueObjects;

public sealed record ReceivingStatus
{
    private static readonly HashSet<string> ValidStatuses =
    [
        "OPEN", "CLOSED", "CANCELLED"
    ];

    public static readonly ReceivingStatus Open = new("OPEN");
    public static readonly ReceivingStatus Closed = new("CLOSED");
    public static readonly ReceivingStatus Cancelled = new("CANCELLED");

    public string Value { get; }

    private ReceivingStatus(string value)
    {
        if (!ValidStatuses.Contains(value))
            throw new ArgumentException($"Invalid receiving status: {value}");
        Value = value;
    }

    public static ReceivingStatus From(string value) => new(value.ToUpperInvariant());

    public override string ToString() => Value;
}
