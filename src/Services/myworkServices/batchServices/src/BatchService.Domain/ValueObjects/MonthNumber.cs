namespace BatchService.Domain.ValueObjects;

/// <summary>Value object ensuring month numbers stay within 1-12.</summary>
public sealed class MonthNumber : IEquatable<MonthNumber>
{
    public int Value { get; }

    public MonthNumber(int value)
    {
        if (value < 1 || value > 12)
            throw new ArgumentOutOfRangeException(nameof(value), "Month number must be between 1 and 12.");
        Value = value;
    }

    public string MonthName => new DateTime(2000, Value, 1).ToString("MMMM");

    public bool Equals(MonthNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is MonthNumber m && Equals(m);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString("D2");
    public static bool operator ==(MonthNumber? left, MonthNumber? right) => Equals(left, right);
    public static bool operator !=(MonthNumber? left, MonthNumber? right) => !Equals(left, right);
}
