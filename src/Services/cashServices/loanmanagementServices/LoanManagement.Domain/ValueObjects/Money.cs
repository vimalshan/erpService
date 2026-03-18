namespace LoanManagement.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
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
            throw new ArgumentException("Money amount cannot be negative.", nameof(amount));

        return new Money(amount, currencyId);
    }

    public Money Add(Money other)
    {
        if (CurrencyId != other.CurrencyId)
            throw new InvalidOperationException("Cannot add money of different currencies.");

        return new Money(Amount + other.Amount, CurrencyId);
    }

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount && CurrencyId == other.CurrencyId;

    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => HashCode.Combine(Amount, CurrencyId);
    public override string ToString() => $"{Amount:N2} [{CurrencyId}]";
}
