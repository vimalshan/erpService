namespace InventoryManagement.Domain.ValueObjects;

public sealed record ProductName
{
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Product name cannot be empty.", nameof(value));
        if (value.Length > 20)
            throw new ArgumentException("Product name cannot exceed 20 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(ProductName name) => name.Value;
    public static explicit operator ProductName(string value) => new(value);
    public override string ToString() => Value;
}
