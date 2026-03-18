namespace InventoryManagement.Domain.ValueObjects;

public sealed record OracleCode
{
    public string Value { get; }

    public OracleCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Oracle code cannot be empty.", nameof(value));
        if (value.Length > 20)
            throw new ArgumentException("Oracle code cannot exceed 20 characters.", nameof(value));
        Value = value.Trim().ToUpperInvariant();
    }

    public static implicit operator string(OracleCode code) => code.Value;
    public static explicit operator OracleCode(string value) => new(value);
    public override string ToString() => Value;
}
