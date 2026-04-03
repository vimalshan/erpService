namespace LoanTransaction.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>, IComparable<Money>
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "INR";

    private Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        Amount = amount;
    }

    public static Money Zero() => new(0);
    public static Money Create(decimal amount) => new(amount);

    public Money Add(Money other) => new(Amount + (other?.Amount ?? throw new ArgumentNullException(nameof(other))));
    public Money Subtract(Money other)
    {
        if (other is null) throw new ArgumentNullException(nameof(other));
        if (other.Amount > Amount) throw new InvalidOperationException("Cannot subtract greater amount.");
        return new(Amount - other.Amount);
    }
    public Money Multiply(decimal factor) => new(Amount * factor);

    public bool IsZero => Amount == 0;
    public bool IsPositive => Amount > 0;

    public override bool Equals(object? obj) => Equals(obj as Money);
    public bool Equals(Money? other) => other is not null && Amount == other.Amount && Currency == other.Currency;
    public int CompareTo(Money? other) => other is null ? 1 : Amount.CompareTo(other.Amount);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    public override string ToString() => $"{Currency} {Amount:N2}";

    public static bool operator ==(Money? l, Money? r) => l?.Equals(r) ?? r is null;
    public static bool operator !=(Money? l, Money? r) => !(l == r);
    public static bool operator <(Money? l, Money? r) => l is not null && r is not null && l.CompareTo(r) < 0;
    public static bool operator >(Money? l, Money? r) => l is not null && r is not null && l.CompareTo(r) > 0;
    public static Money operator +(Money l, Money r) => l.Add(r);
    public static Money operator -(Money l, Money r) => l.Subtract(r);
}
