namespace PayrollServices.Domain.ValueObjects;

/// <summary>
/// Value object representing a payroll month in YYYY-MM format
/// </summary>
public class PayrollMonth : IEquatable<PayrollMonth>
{
    private const string DateFormat = "yyyy-MM";
    
    public string Value { get; init; }

    public int Year { get; init; }
    public int Month { get; init; }

    public PayrollMonth(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Payroll month cannot be empty.", nameof(value));

        if (!DateTime.TryParseExact(value, DateFormat, null, System.Globalization.DateTimeStyles.None, out var date))
            throw new ArgumentException($"Invalid payroll month format. Expected: {DateFormat}", nameof(value));

        Value = value;
        Year = date.Year;
        Month = date.Month;
    }

    public static PayrollMonth FromDateTime(DateTime dateTime) => new(dateTime.ToString(DateFormat));

    public static PayrollMonth Current => FromDateTime(DateTime.Now);

    public DateTime ToDateTime() => new(Year, Month, 1);

    public override bool Equals(object? obj) => Equals(obj as PayrollMonth);

    public bool Equals(PayrollMonth? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}
