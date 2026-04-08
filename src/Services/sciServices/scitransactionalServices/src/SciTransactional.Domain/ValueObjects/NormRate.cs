using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.ValueObjects;

public sealed class NormRate : ValueObject
{
    public int Value { get; }

    private NormRate(int value) => Value = value;

    public static NormRate Create(int value)
    {
        if (value < 0)
            throw new ArgumentException("Norm rate cannot be negative.");
        return new NormRate(value);
    }

    public static NormRate Zero => new(0);

    public static implicit operator int(NormRate rate) => rate.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
