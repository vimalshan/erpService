namespace DealTicketing.Domain.ValueObjects;

public sealed record Money(decimal Amount, string CurrencyCode)
{
    public static Money Zero(string currencyCode) => new(0, currencyCode);

    public Money Add(Money other)
    {
        if (CurrencyCode != other.CurrencyCode)
            throw new InvalidOperationException("Cannot add amounts in different currencies.");
        return new Money(Amount + other.Amount, CurrencyCode);
    }
}

public sealed record ExchangeRate(decimal Rate, string BaseCurrency, string QuoteCurrency)
{
    public decimal Convert(decimal amount) => amount * Rate;
}
