namespace OrderScheduleService.Domain.ValueObjects;

using OrderScheduleService.Domain.Common;

public class OrderNumber : ValueObject
{
    public decimal Number { get; private set; }
    public string? Code { get; private set; }

    public OrderNumber(decimal number, string? code = null)
    {
        if (number <= 0)
            throw new ArgumentException("Order number must be greater than zero", nameof(number));
        
        Number = number;
        Code = code;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
        yield return Code ?? string.Empty;
    }

    public override string ToString() => $"{Number}{Code}";
}
