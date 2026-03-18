namespace AccountingService.Domain.ValueObjects;

public sealed record TrustCode
{
    public string Value { get; }

    public TrustCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 3)
            throw new ArgumentException("TrustCode must be exactly 3 characters.", nameof(value));
        Value = value.ToUpperInvariant();
    }

    public static implicit operator string(TrustCode code) => code.Value;
    public override string ToString() => Value;
}
