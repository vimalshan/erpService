namespace ProductionManagement.Domain.ValueObjects;

using ProductionManagement.Domain.Common;

public class PlantName : ValueObject
{
    public string Value { get; private set; }

    private PlantName() { Value = string.Empty; }

    public PlantName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Plant name cannot be empty.", nameof(value));
        if (value.Length > 60)
            throw new ArgumentException("Plant name cannot exceed 60 characters.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(PlantName name) => name.Value;
}
