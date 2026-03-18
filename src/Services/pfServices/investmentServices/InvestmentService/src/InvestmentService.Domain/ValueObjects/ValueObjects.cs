namespace InvestmentService.Domain.ValueObjects;

public record Money(decimal Amount)
{
    public static Money Zero => new(0);
    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount);
    public static Money operator *(Money a, decimal factor) => new(a.Amount * factor);
}

public record InterestRate(decimal Rate)
{
    public decimal AnnualRate => Rate;
}

public record DateRange(DateTime From, DateTime To)
{
    public int Days => (To - From).Days;
}

public record InvestmentNumber(long Value)
{
    public override string ToString() => Value.ToString();
}
