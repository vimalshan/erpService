namespace BankService.Domain.ValueObjects;

public record Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    public static implicit operator decimal(Money money) => money.Amount;
    public override string ToString() => Amount.ToString("N0");
}
