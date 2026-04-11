namespace TransactionService.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    private Money(decimal amount) => Amount = Math.Round(amount, 0);

    public static Money From(decimal amount) => new(amount);
    public static Money Zero => new(0);

    public Money Add(Money other) => new(Amount + other.Amount);
    public Money Subtract(Money other) => new(Amount - other.Amount);

    public static implicit operator decimal(Money money) => money.Amount;
    public override string ToString() => Amount.ToString("N0");
}
