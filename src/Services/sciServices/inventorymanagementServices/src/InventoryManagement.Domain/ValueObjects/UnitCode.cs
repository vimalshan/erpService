namespace InventoryManagement.Domain.ValueObjects;

public sealed record UnitCode
{
    public string Value { get; }

    public UnitCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Unit code cannot be empty.", nameof(value));
        if (value.Length > 3)
            throw new ArgumentException("Unit code cannot exceed 3 characters.", nameof(value));
        Value = value.Trim().ToUpperInvariant();
    }

    public static implicit operator string(UnitCode code) => code.Value;
    public static explicit operator UnitCode(string value) => new(value);
    public override string ToString() => Value;
}
