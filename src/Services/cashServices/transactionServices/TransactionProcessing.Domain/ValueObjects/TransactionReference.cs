namespace TransactionProcessing.Domain.ValueObjects;

public sealed record TransactionReference
{
    public string Value { get; init; }

    public TransactionReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Transaction reference cannot be empty.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Transaction reference cannot exceed 100 characters.", nameof(value));
        Value = value;
    }

    public static TransactionReference Generate(string prefix)
        => new($"{prefix}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}");

    public override string ToString() => Value;
}
