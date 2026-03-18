using EligibilityService.Domain.Common;

namespace EligibilityService.Domain.ValueObjects;

public sealed class ItemCode : ValueObject
{
    public decimal Value { get; }

    private ItemCode(decimal value) => Value = value;

    public static ItemCode Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Item code must be positive.");
        return new ItemCode(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
