namespace MamAllocationService.Domain.ValueObjects;

public record QuantityValue(decimal Value)
{
    public static QuantityValue Zero => new(0m);

    public static implicit operator decimal(QuantityValue q) => q.Value;
    public static explicit operator QuantityValue(decimal v) => new(v);
}
