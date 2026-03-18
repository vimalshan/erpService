using CurrencyManagement.Domain.Common;

namespace CurrencyManagement.Domain.ValueObjects;

/// <summary>
/// Value object representing an exchange rate as a decimal value
/// </summary>
public class ExchangeRateValue : ValueObject
{
    public decimal Value { get; }

    private ExchangeRateValue(decimal value)
    {
        Value = value;
    }

    public static ExchangeRateValue Create(decimal rate)
    {
        if (rate <= 0)
            throw new ArgumentException("Exchange rate must be greater than 0", nameof(rate));

        return new ExchangeRateValue(rate);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString("F6");
}
