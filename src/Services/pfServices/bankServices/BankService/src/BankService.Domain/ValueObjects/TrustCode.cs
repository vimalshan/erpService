namespace BankService.Domain.ValueObjects;

public record TrustCode
{
    public string Value { get; }

    public TrustCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Trust code must be non-empty and at most 3 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(TrustCode code) => code.Value;
    public override string ToString() => Value;
}
