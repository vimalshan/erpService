namespace ProductService.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    public override string ToString() => Amount.ToString("F4");
}
