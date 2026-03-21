namespace ProductService.Domain.ValueObjects;

public sealed record Sku
{
    public string Value { get; }

    public Sku(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException("SKU must be non-empty and at most 50 characters.", nameof(value));
        Value = value;
    }

    public override string ToString() => Value;
}
