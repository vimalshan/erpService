namespace ProductionManagement.Domain.ValueObjects;

using ProductionManagement.Domain.Common;

public class NormRate : ValueObject
{
    public int Value { get; private set; }

    private NormRate() { }

    public NormRate(int value)
    {
        if (value < 0)
            throw new ArgumentException("Norm rate cannot be negative.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator int(NormRate rate) => rate.Value;
}
