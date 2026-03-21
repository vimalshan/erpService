namespace RackingSystem.Domain.ValueObjects;

public sealed record BinStatus
{
    public static readonly BinStatus Available = new("AVAILABLE");
    public static readonly BinStatus Occupied  = new("OCCUPIED");
    public static readonly BinStatus Blocked   = new("BLOCKED");
    public static readonly BinStatus Full      = new("FULL");

    private static readonly HashSet<string> ValidStatuses =
    [
        Available.Value, Occupied.Value, Blocked.Value, Full.Value
    ];

    public string Value { get; }

    private BinStatus(string value) => Value = value;

    public static BinStatus From(string value)
    {
        var normalised = value?.ToUpperInvariant() ?? string.Empty;
        if (!ValidStatuses.Contains(normalised))
            throw new ArgumentException($"'{value}' is not a valid BinStatus.", nameof(value));
        return new BinStatus(normalised);
    }

    public static implicit operator string(BinStatus bs) => bs.Value;
    public override string ToString() => Value;
}
