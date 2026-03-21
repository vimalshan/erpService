namespace BookingService.Domain.ValueObjects;

public sealed record BookingType
{
    public static readonly BookingType Ticket = new("TKT");
    public static readonly BookingType Stay = new("STY");
    public static readonly BookingType Cab = new("CAB");

    public string Value { get; }

    private BookingType(string value) => Value = value;

    public static BookingType From(string value) => value.ToUpperInvariant() switch
    {
        "TKT" => Ticket,
        "STY" => Stay,
        "CAB" => Cab,
        _ => throw new ArgumentException($"Invalid booking type: {value}")
    };

    public override string ToString() => Value;
}
