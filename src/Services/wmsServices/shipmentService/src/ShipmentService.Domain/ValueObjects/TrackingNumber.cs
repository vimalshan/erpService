using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.ValueObjects;

public sealed class TrackingNumber : ValueObject
{
    public string Value { get; }

    private TrackingNumber(string value) => Value = value;

    public static TrackingNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Tracking number cannot be empty.", nameof(value));
        if (value.Length > 100)
            throw new ArgumentException("Tracking number cannot exceed 100 characters.", nameof(value));
        return new TrackingNumber(value.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
