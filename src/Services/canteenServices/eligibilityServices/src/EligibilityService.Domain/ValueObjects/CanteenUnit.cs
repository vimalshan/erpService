using EligibilityService.Domain.Common;

namespace EligibilityService.Domain.ValueObjects;

public sealed class CanteenUnit : ValueObject
{
    public long Value { get; }

    private CanteenUnit(long value) => Value = value;

    public static CanteenUnit Create(long value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Canteen unit code must be positive.");
        return new CanteenUnit(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
