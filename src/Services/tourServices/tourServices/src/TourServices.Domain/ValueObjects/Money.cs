namespace TourServices.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
        Amount = Math.Round(amount, 0);
    }

    public static Money Zero => new(0m);

    public Money Add(Money other) => new(Amount + other.Amount);
    public Money Divide(int divisor) => divisor == 0 ? Zero : new(Amount / divisor);

    public bool Equals(Money? other) => other is not null && Amount == other.Amount;
    public override bool Equals(object? obj) => obj is Money m && Equals(m);
    public override int GetHashCode() => Amount.GetHashCode();
    public override string ToString() => Amount.ToString("F0");
    public static implicit operator decimal(Money m) => m.Amount;
}
