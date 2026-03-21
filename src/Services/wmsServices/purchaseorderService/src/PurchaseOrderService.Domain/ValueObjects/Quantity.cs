namespace PurchaseOrderService.Domain.ValueObjects;

public record Quantity
{
    public decimal Value { get; }

    public Quantity(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Quantity cannot be negative.", nameof(value));
        Value = value;
    }

    public static Quantity Zero => new(0);

    public static Quantity operator +(Quantity a, Quantity b) => new(a.Value + b.Value);
    public static Quantity operator -(Quantity a, Quantity b) => new(a.Value - b.Value);

    public static implicit operator decimal(Quantity quantity) => quantity.Value;
    public override string ToString() => Value.ToString("F3");
}
