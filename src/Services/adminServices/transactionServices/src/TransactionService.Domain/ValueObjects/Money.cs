namespace TransactionService.Domain.ValueObjects;

using TransactionService.Domain.Common;

public sealed class Money : ValueObject
{
    public long Amount { get; }

    public Money(long amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");
        Amount = amount;
    }

    public static Money Zero => new(0);

    public Money Add(Money other) => new(Amount + other.Amount);
    public Money Subtract(Money other) => new(Amount - other.Amount);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }

    public static implicit operator long(Money money) => money.Amount;
    public static implicit operator Money(long amount) => new(amount);
}
