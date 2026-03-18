namespace BankService.Domain.ValueObjects;

public record BankCode
{
    public string Value { get; }

    public BankCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 6)
            throw new ArgumentException("Bank code must be non-empty and at most 6 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(BankCode code) => code.Value;
    public override string ToString() => Value;
}
