using LookupService.Domain.Common;

namespace LookupService.Domain.ValueObjects;

public class LovTypeCode : ValueObject
{
    public string Value { get; }

    private LovTypeCode(string value)
    {
        Value = value;
    }

    public static LovTypeCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length > 3)
            throw new ArgumentException("LOV Type code must be 1-3 characters.", nameof(code));

        return new LovTypeCode(code.PadRight(3));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.Trim();
}
