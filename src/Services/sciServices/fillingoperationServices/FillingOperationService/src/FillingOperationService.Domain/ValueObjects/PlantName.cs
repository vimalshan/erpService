namespace FillingOperationService.Domain.ValueObjects;

public sealed class PlantName
{
    public string Value { get; }

    private PlantName(string value) => Value = value;

    public static PlantName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Plant name cannot be empty.", nameof(value));
        if (value.Length > 40)
            throw new ArgumentException("Plant name cannot exceed 40 characters.", nameof(value));
        return new PlantName(value.Trim());
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is PlantName other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
