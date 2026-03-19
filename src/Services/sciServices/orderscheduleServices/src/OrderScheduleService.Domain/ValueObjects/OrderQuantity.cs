namespace OrderScheduleService.Domain.ValueObjects;

using OrderScheduleService.Domain.Common;

public class OrderQuantity : ValueObject
{
    public decimal Quantity { get; private set; }
    public string? UnitOfMeasure { get; private set; }

    public OrderQuantity(decimal quantity, string? unitOfMeasure)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        Quantity = quantity;
        UnitOfMeasure = unitOfMeasure;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Quantity;
        yield return UnitOfMeasure ?? string.Empty;
    }
}
