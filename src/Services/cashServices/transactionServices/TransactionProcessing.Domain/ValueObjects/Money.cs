namespace TransactionProcessing.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; init; }
    public long? CurrencyId { get; init; }

    public Money(decimal amount, long? currencyId = null)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Amount = amount;
        CurrencyId = currencyId;
    }

    public static Money Zero(long? currencyId = null) => new(0, currencyId);

    public Money Add(Money other) => new(Amount + other.Amount, CurrencyId);
    public Money Subtract(Money other) => new(Amount - other.Amount, CurrencyId);
}
