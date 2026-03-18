namespace ReportingService.Domain.ValueObjects;

/// <summary>
/// Rating value object for performance ratings
/// </summary>
public class Rating
{
    public decimal Value { get; set; }

    public Rating(decimal value)
    {
        if (value < 0 || value > 5)
            throw new ArgumentException("Rating must be between 0 and 5", nameof(value));
        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Rating other)
            return Value == other.Value;
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString("F2");
}
