namespace LoanApplication.Domain.ValueObjects;

/// <summary>
/// Money value object for handling loan amounts
/// </summary>
public class Money : IEquatable<Money>, IComparable<Money>
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "INR";

    private Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
    }

    public static Money Zero() => new(0);
    public static Money Create(decimal amount) => new(amount);

    public Money Add(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        return new Money(Amount + other.Amount);
    }

    public Money Subtract(Money other)
    {
        if (other == null)
            throw new ArgumentNullException(nameof(other));

        if (other.Amount > Amount)
            throw new InvalidOperationException("Cannot subtract amount greater than current amount");

        return new Money(Amount - other.Amount);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0)
            throw new ArgumentException("Multiplier cannot be negative", nameof(factor));

        return new Money(Amount * factor);
    }

    public bool IsZero => Amount == 0;
    public bool IsPositive => Amount > 0;
    public bool IsNegative => Amount < 0;

    public override bool Equals(object? obj) => Equals(obj as Money);

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount && Currency == other.Currency;

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        return Amount.CompareTo(other.Amount);
    }

    public override int GetHashCode() => Amount.GetHashCode();

    public override string ToString() => $"{Amount:C}";

    public static bool operator ==(Money? left, Money? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Money? left, Money? right) =>
        !(left == right);

    public static bool operator <(Money? left, Money? right) =>
        left is not null && right is not null && left.CompareTo(right) < 0;

    public static bool operator <=(Money? left, Money? right) =>
        left is not null && right is not null && left.CompareTo(right) <= 0;

    public static bool operator >(Money? left, Money? right) =>
        left is not null && right is not null && left.CompareTo(right) > 0;

    public static bool operator >=(Money? left, Money? right) =>
        left is not null && right is not null && left.CompareTo(right) >= 0;

    public static Money operator +(Money? left, Money? right) =>
        left?.Add(right ?? throw new ArgumentNullException(nameof(right))) ?? throw new ArgumentNullException(nameof(left));

    public static Money operator -(Money? left, Money? right) =>
        left?.Subtract(right ?? throw new ArgumentNullException(nameof(right))) ?? throw new ArgumentNullException(nameof(left));
}
