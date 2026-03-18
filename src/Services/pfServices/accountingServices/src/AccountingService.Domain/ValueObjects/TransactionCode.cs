namespace AccountingService.Domain.ValueObjects;

public sealed record TransactionCode
{
    public string Value { get; }

    public TransactionCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("TransactionCode must be at most 3 characters.", nameof(value));
        Value = value.ToUpperInvariant();
    }

    public static implicit operator string(TransactionCode code) => code.Value;
    public override string ToString() => Value;
}
