namespace OrderService.Domain.ValueObjects;

public sealed record OrderNumber
{
    public string Value { get; }

    public OrderNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Order number cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Order number cannot exceed 50 characters.", nameof(value));
        Value = value;
    }

    public static OrderNumber Generate() => new($"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}");

    public override string ToString() => Value;
}
