using EligibilityService.Domain.Common;

namespace EligibilityService.Domain.ValueObjects;

public sealed class ShiftCode : ValueObject
{
    public char Value { get; }

    private ShiftCode(char value) => Value = value;

    public static ShiftCode Create(char value)
    {
        if (!char.IsLetterOrDigit(value))
            throw new ArgumentException("Shift code must be a letter or digit.", nameof(value));
        return new ShiftCode(char.ToUpperInvariant(value));
    }

    public static ShiftCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 1)
            throw new ArgumentException("Shift code must be exactly one character.", nameof(value));
        return Create(value[0]);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
