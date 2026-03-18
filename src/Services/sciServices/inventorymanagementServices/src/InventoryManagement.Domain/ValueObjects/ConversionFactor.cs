namespace InventoryManagement.Domain.ValueObjects;

public sealed record ConversionFactor
{
    public decimal Value { get; }

    public ConversionFactor(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Conversion factor must be positive.", nameof(value));
        Value = value;
    }

    public static implicit operator decimal(ConversionFactor factor) => factor.Value;
    public static explicit operator ConversionFactor(decimal value) => new(value);
    public override string ToString() => Value.ToString();
}
