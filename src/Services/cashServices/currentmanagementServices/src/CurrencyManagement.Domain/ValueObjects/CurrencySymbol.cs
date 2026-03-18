using CurrencyManagement.Domain.Common;

namespace CurrencyManagement.Domain.ValueObjects;

/// <summary>
/// Value object representing a currency symbol (e.g., $, €, £, ₹)
/// </summary>
public class CurrencySymbol : ValueObject
{
    public string Value { get; }

    private CurrencySymbol(string value)
    {
        Value = value;
    }

    public static CurrencySymbol Create(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Currency symbol cannot be empty", nameof(symbol));

        if (symbol.Length > 25)
            throw new ArgumentException("Currency symbol cannot exceed 25 characters", nameof(symbol));

        return new CurrencySymbol(symbol.Trim());
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
