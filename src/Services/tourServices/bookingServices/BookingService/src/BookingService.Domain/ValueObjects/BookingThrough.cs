namespace BookingService.Domain.ValueObjects;

public sealed record BookingThrough
{
    public static readonly BookingThrough Self = new("Self");
    public static readonly BookingThrough Admin = new("Admin");
    public static readonly BookingThrough Subordinate = new("Subordinate");

    public string Value { get; }

    private BookingThrough(string value) => Value = value;

    public static BookingThrough From(string value) => value switch
    {
        "Self" => Self,
        "Admin" => Admin,
        "Subordinate" => Subordinate,
        _ => throw new ArgumentException($"Invalid booking through value: {value}")
    };

    public override string ToString() => Value;
}
