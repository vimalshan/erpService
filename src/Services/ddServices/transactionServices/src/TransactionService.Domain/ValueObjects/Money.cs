namespace TransactionService.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        Amount = amount;
    }

    public override bool Equals(object? obj) => obj is Money other && Amount == other.Amount;
    public override int GetHashCode() => Amount.GetHashCode();
    public override string ToString() => Amount.ToString("N2");
}
