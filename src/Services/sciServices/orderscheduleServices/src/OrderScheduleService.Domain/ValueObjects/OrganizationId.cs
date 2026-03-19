namespace OrderScheduleService.Domain.ValueObjects;

using OrderScheduleService.Domain.Common;

public class OrganizationId : ValueObject
{
    public decimal Value { get; private set; }

    public OrganizationId(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Organization ID must be greater than zero", nameof(value));
        
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
