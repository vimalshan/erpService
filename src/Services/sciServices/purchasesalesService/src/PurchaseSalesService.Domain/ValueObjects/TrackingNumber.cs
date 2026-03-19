using PurchaseSalesService.Domain.Common;

namespace PurchaseSalesService.Domain.ValueObjects;

public sealed class TrackingNumber : ValueObject
{
    public long Value { get; }

    private TrackingNumber(long value) => Value = value;

    public static TrackingNumber Create(long value)
    {
        if (value <= 0) throw new ArgumentException("Tracking number must be positive.", nameof(value));
        return new TrackingNumber(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
