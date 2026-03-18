namespace ReviewService.Domain.ValueObjects;

public sealed record Rating
{
    public decimal Value { get; }

    public Rating(decimal value)
    {
        if (value < 0 || value > 10)
            throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 0 and 10.");
        Value = value;
    }

    public static Rating None => new(0);
    public static Rating FromDecimal(decimal value) => new(value);
    public override string ToString() => Value.ToString("F2");
}
