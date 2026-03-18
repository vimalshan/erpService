namespace TaxService.Domain.ValueObjects;

/// <summary>
/// Value object for monetary amounts
/// </summary>
public sealed record Money(decimal Amount, string Currency = "INR") : IEquatable<Money>
{
    public static Money Zero(string currency = "INR") => new(0m, currency);

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add amounts in different currencies");
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot subtract amounts in different currencies");
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal multiplier) 
        => new(money.Amount * multiplier, money.Currency);

    public static bool operator >(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare amounts in different currencies");
        return left.Amount > right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare amounts in different currencies");
        return left.Amount < right.Amount;
    }

    public bool Equals(Money? other) 
        => other is not null && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
}
