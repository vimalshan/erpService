namespace BookingService.Domain.ValueObjects;

public sealed record ConfirmationStatus
{
    public static readonly ConfirmationStatus Pending = new("Pending");
    public static readonly ConfirmationStatus Confirmed = new("Confirmed");
    public static readonly ConfirmationStatus Cancelled = new("Cancelled");

    public string Value { get; }

    private ConfirmationStatus(string value) => Value = value;

    public static ConfirmationStatus From(string value) => value switch
    {
        "Pending" => Pending,
        "Confirmed" => Confirmed,
        "Cancelled" => Cancelled,
        _ => throw new ArgumentException($"Invalid confirmation status: {value}")
    };

    public override string ToString() => Value;
}
