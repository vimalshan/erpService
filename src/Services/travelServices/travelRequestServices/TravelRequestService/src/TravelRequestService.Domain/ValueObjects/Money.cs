using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }

    private Money() { }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    public static Money Zero => new(0);

    public Money Add(Money other) => new(Amount + other.Amount);
    public Money Subtract(Money other) => new(Amount - other.Amount);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
    }
}
