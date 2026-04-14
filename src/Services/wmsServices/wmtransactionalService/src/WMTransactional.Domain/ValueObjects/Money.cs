namespace WMTransactional.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Money amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    public static Money Zero => new(0);

    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount);
    public static Money operator *(Money a, decimal multiplier) => new(a.Amount * multiplier);

    public static implicit operator decimal(Money m) => m.Amount;
    public static explicit operator Money(decimal d) => new(d);

    public override string ToString() => Amount.ToString("N4");
}
