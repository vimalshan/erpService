using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.ValueObjects;

public class LoanLimit : ValueObject
{
    public long MinimumLimit { get; }
    public long MaximumLimit { get; }

    public LoanLimit(long minimumLimit, long maximumLimit)
    {
        if (minimumLimit < 0) throw new ArgumentException("Minimum limit cannot be negative.", nameof(minimumLimit));
        if (maximumLimit < minimumLimit) throw new ArgumentException("Maximum limit must be >= minimum limit.", nameof(maximumLimit));
        MinimumLimit = minimumLimit;
        MaximumLimit = maximumLimit;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return MinimumLimit;
        yield return MaximumLimit;
    }
}
