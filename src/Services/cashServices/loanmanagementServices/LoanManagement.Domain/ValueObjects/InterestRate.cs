using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.ValueObjects;

public sealed class InterestRate : IEquatable<InterestRate>
{
    public InterestRateType RateType { get; }
    public decimal Percentage { get; }
    public long? FloatTypeId { get; }

    private InterestRate(InterestRateType rateType, decimal percentage, long? floatTypeId)
    {
        RateType = rateType;
        Percentage = percentage;
        FloatTypeId = floatTypeId;
    }

    public static InterestRate CreateFixed(decimal percentage)
    {
        if (percentage <= 0 || percentage > 100)
            throw new ArgumentException("Fixed rate must be between 0 and 100.", nameof(percentage));

        return new InterestRate(InterestRateType.Fixed, percentage, null);
    }

    public static InterestRate CreateFloating(decimal spread, long floatTypeId)
    {
        if (spread < 0)
            throw new ArgumentException("Floating spread cannot be negative.", nameof(spread));

        return new InterestRate(InterestRateType.Floating, spread, floatTypeId);
    }

    public bool Equals(InterestRate? other) =>
        other is not null &&
        RateType == other.RateType &&
        Percentage == other.Percentage &&
        FloatTypeId == other.FloatTypeId;

    public override bool Equals(object? obj) => obj is InterestRate ir && Equals(ir);
    public override int GetHashCode() => HashCode.Combine(RateType, Percentage, FloatTypeId);
    public override string ToString() =>
        RateType == InterestRateType.Fixed
            ? $"Fixed {Percentage}%"
            : $"Floating +{Percentage}% over [{FloatTypeId}]";
}
