namespace ReceivingService.Domain.ValueObjects;

public sealed class ReceivingStatus
{
    public static readonly ReceivingStatus Open       = new("OPEN");
    public static readonly ReceivingStatus Closed     = new("CLOSED");
    public static readonly ReceivingStatus Cancelled  = new("CANCELLED");

    public string Value { get; }

    private ReceivingStatus(string value) => Value = value;

    public static ReceivingStatus From(string value) =>
        value?.ToUpperInvariant() switch
        {
            "OPEN"      => Open,
            "CLOSED"    => Closed,
            "CANCELLED" => Cancelled,
            _           => throw new ArgumentException($"Unknown receiving status: {value}", nameof(value))
        };

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ReceivingStatus s && Value == s.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
