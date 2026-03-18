using LocationServices.Domain.Common;

namespace LocationServices.Domain.ValueObjects;

/// <summary>Value object representing a valid LocationId (DDD)</summary>
public sealed class LocationId : ValueObject
{
    public decimal Value { get; }

    private LocationId(decimal value) => Value = value;

    public static LocationId Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("LocationId must be greater than zero.", nameof(value));
        return new LocationId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public static implicit operator decimal(LocationId id) => id.Value;
    public static explicit operator LocationId(decimal value) => Create(value);
}
