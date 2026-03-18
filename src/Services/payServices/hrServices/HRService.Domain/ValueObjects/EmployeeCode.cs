namespace HRService.Domain.ValueObjects;

public class EmployeeCode : Common.ValueObject
{
    public string Value { get; }

    private EmployeeCode(string value)
    {
        Value = value;
    }

    public static EmployeeCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Employee code cannot be empty", nameof(code));

        return new EmployeeCode(code);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToUpperInvariant();
    }

    public override string ToString() => Value;
}
