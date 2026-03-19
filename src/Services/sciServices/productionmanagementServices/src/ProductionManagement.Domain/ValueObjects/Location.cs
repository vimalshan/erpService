namespace ProductionManagement.Domain.ValueObjects;

using ProductionManagement.Domain.Common;

public class Location : ValueObject
{
    public string Value { get; private set; }

    private Location() { Value = string.Empty; }

    public Location(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Location cannot be empty.", nameof(value));
        if (value.Length > 25)
            throw new ArgumentException("Location cannot exceed 25 characters.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(Location location) => location.Value;
}
