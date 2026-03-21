using TourPlanService.Domain.Common;

namespace TourPlanService.Domain.ValueObjects;

public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "INR")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);
        if (amount < 0) throw new ArgumentException("Amount cannot be negative.");
        return new Money(amount, currency.ToUpperInvariant());
    }

    public static Money Zero(string currency = "INR") => new(0, currency.ToUpperInvariant());

    public Money Add(Money other)
    {
        if (Currency != other.Currency) throw new InvalidOperationException("Cannot add different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Amount:N2} {Currency}";
}
