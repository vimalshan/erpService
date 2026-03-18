using CurrencyManagement.Domain.Common;

namespace CurrencyManagement.Domain.ValueObjects;

/// <summary>
/// Value object representing a monetary amount with a currency
/// </summary>
public class Money : ValueObject
{
    public decimal Amount { get; }
    public long CurrencyId { get; }

    private Money(decimal amount, long currencyId)
    {
        Amount = amount;
        CurrencyId = currencyId;
    }

    public static Money Create(decimal amount, long currencyId)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (currencyId <= 0)
            throw new ArgumentException("Currency ID must be positive", nameof(currencyId));

        return new Money(amount, currencyId);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return CurrencyId;
    }

    public override string ToString() => $"{Amount} (Currency: {CurrencyId})";
}
