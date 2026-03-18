using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
    }

    public static Money Zero => new(0);
    public static Money From(decimal amount) => new(amount);

    public Money Add(Money other)
    {
        return new Money(Amount + other.Amount);
    }

    public Money Subtract(Money other)
    {
        if (other.Amount > Amount)
            throw new InvalidOperationException("Cannot subtract more than available amount");

        return new Money(Amount - other.Amount);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0)
            throw new ArgumentException("Multiplier cannot be negative", nameof(factor));

        return new Money(Amount * factor);
    }

    public bool IsGreaterThan(Money other) => Amount > other.Amount;
    public bool IsLessThan(Money other) => Amount < other.Amount;
    public bool IsEqual(Money other) => Amount == other.Amount;

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }
}
