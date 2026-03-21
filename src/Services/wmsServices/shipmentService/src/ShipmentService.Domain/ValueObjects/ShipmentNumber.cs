using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.ValueObjects;

public sealed class ShipmentNumber : ValueObject
{
    public string Value { get; }

    private ShipmentNumber(string value) => Value = value;

    public static ShipmentNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Shipment number cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Shipment number cannot exceed 50 characters.", nameof(value));
        return new ShipmentNumber(value.Trim().ToUpper());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(ShipmentNumber number) => number.Value;
}
