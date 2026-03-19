namespace StrategicStock.Domain.ValueObjects;

using Common;

public sealed class StockQuantity : ValueObject
{
    public long Value { get; }

    private StockQuantity(long value) => Value = value;

    public static StockQuantity Create(long value)
    {
        if (value < 0)
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(value));

        return new StockQuantity(value);
    }

    public static StockQuantity Zero => new(0);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator long(StockQuantity qty) => qty.Value;
}
