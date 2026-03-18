namespace AlertsNotifications.Domain.ValueObjects;

public sealed record CircularStatus
{
    public static readonly CircularStatus Draft = new("D");
    public static readonly CircularStatus Pending = new("P");
    public static readonly CircularStatus Approved = new("A");
    public static readonly CircularStatus Rejected = new("R");
    public static readonly CircularStatus Cancelled = new("C");

    public string Value { get; }

    private CircularStatus(string value) => Value = value;

    public static CircularStatus From(string value)
    {
        return value switch
        {
            "D" => Draft,
            "P" => Pending,
            "A" => Approved,
            "R" => Rejected,
            "C" => Cancelled,
            _ => throw new ArgumentException($"Invalid circular status: {value}", nameof(value))
        };
    }

    public override string ToString() => Value;
}
