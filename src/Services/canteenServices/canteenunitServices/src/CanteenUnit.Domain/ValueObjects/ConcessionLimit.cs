namespace CanteenUnit.Domain.ValueObjects;

public sealed class ConcessionLimit : IEquatable<ConcessionLimit>
{
    public decimal? MaxValue { get; }
    public decimal? MinValue { get; }

    private ConcessionLimit(decimal? maxValue, decimal? minValue)
    {
        MaxValue = maxValue;
        MinValue = minValue;
    }

    public static ConcessionLimit Create(decimal? maxValue, decimal? minValue)
    {
        if (maxValue.HasValue && minValue.HasValue && maxValue < minValue)
            throw new ArgumentException("Max concession limit cannot be less than min limit.");
        return new ConcessionLimit(maxValue, minValue);
    }

    public bool Equals(ConcessionLimit? other) =>
        other is not null && MaxValue == other.MaxValue && MinValue == other.MinValue;

    public override bool Equals(object? obj) => obj is ConcessionLimit cl && Equals(cl);
    public override int GetHashCode() => HashCode.Combine(MaxValue, MinValue);
}
