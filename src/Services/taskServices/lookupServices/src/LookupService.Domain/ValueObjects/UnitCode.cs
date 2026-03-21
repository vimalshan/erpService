using LookupService.Domain.Common;

namespace LookupService.Domain.ValueObjects;

public class UnitCode : ValueObject
{
    public string Value { get; }

    private UnitCode(string value)
    {
        Value = value;
    }

    public static UnitCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length > 3)
            throw new ArgumentException("Unit code must be 1-3 characters.", nameof(code));

        return new UnitCode(code.PadRight(3));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.Trim();
}
