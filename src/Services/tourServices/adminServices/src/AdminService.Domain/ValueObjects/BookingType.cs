namespace AdminService.Domain.ValueObjects;

public record BookingType
{
    public string Value { get; }

    public static readonly BookingType Ticket = new("TKT");
    public static readonly BookingType Stationery = new("STY");
    public static readonly BookingType Cab = new("CAB");
    public static readonly BookingType Forex = new("FRX");

    private static readonly HashSet<string> ValidValues = new() { "TKT", "STY", "CAB", "FRX" };

    private BookingType(string value) => Value = value;

    public static BookingType From(string value)
    {
        if (!ValidValues.Contains(value))
            throw new ArgumentException($"Invalid booking type: {value}. Valid values: {string.Join(", ", ValidValues)}");
        return new BookingType(value);
    }

    public override string ToString() => Value;
}
