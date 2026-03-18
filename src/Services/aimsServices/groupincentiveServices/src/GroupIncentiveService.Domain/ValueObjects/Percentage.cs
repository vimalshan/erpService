using GroupIncentiveService.Domain.Exceptions;

namespace GroupIncentiveService.Domain.ValueObjects;

public sealed record Percentage
{
    public decimal Value { get; }

    public Percentage(decimal value)
    {
        if (value < 0 || value > 100)
            throw new DomainException($"Percentage must be between 0 and 100. Provided: {value}");
        Value = Math.Round(value, 2);
    }

    public static Percentage Zero => new(0m);
    public static Percentage Full => new(100m);

    public Money ApplyTo(Money money) => new(Math.Round(money.Amount * Value / 100m, 2));

    public override string ToString() => $"{Value:N2}%";
}
