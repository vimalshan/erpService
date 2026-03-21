namespace ConfigService.Domain.ValueObjects;

using ConfigService.Domain.Common;

public class CurrencyCode : ValueObject
{
    public string Value { get; }

    public CurrencyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Currency code must be 1-3 characters.", nameof(value));
        Value = value.ToUpperInvariant();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(CurrencyCode code) => code.Value;
}
