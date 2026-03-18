namespace CashManagement.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }

    private Money() { }

    public Money(decimal amount)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    public static Money Zero => new(0m);

    public static Money Of(decimal amount) => new(amount);

    public Money Add(Money other) => new(Amount + other.Amount);
    public Money Subtract(Money other)
    {
        if (Amount < other.Amount) throw new InvalidOperationException("Result would be negative.");
        return new(Amount - other.Amount);
    }

    public bool IsGreaterThan(Money other) => Amount > other.Amount;

    public override string ToString() => Amount.ToString("N2");
}
