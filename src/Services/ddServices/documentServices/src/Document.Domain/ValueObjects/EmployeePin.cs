namespace Document.Domain.ValueObjects;

public sealed class EmployeePin : Common.ValueObject
{
    public decimal Value { get; }

    private EmployeePin(decimal value) => Value = value;

    public static EmployeePin Of(decimal value)
    {
        if (value <= 0) throw new Exceptions.DomainException("Employee pin must be positive.");
        return new EmployeePin(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
