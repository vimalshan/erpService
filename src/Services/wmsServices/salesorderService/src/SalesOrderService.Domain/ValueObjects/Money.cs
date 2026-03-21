namespace SalesOrderService.Domain.ValueObjects;

public sealed class Money
{
    private Money() { Currency = "USD"; } // required for EF Core owned entity materialisation

    public Money(decimal amount, string currency = "USD")
    {
        Amount   = amount;
        Currency = currency;
    }

    public decimal Amount   { get; private set; }
    public string  Currency { get; private set; }

    public static Money Zero(string currency = "USD") => new(0, currency);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add money of different currencies.");
        return new Money(Amount + other.Amount, Currency);
    }

    public override string  ToString()              => $"{Amount:F2} {Currency}";
    public override bool    Equals(object? obj)     => obj is Money m && Amount == m.Amount && Currency == m.Currency;
    public override int     GetHashCode()           => HashCode.Combine(Amount, Currency);
}
