namespace FillingOperationService.Domain.ValueObjects;

public sealed class Location
{
    public string Value { get; }

    private Location(string value) => Value = value;

    public static Location Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Location cannot be empty.", nameof(value));
        if (value.Length > 20)
            throw new ArgumentException("Location cannot exceed 20 characters.", nameof(value));
        return new Location(value.Trim());
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is Location other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
