using CompensationService.Domain.Common;

namespace CompensationService.Domain.ValueObjects;

/// <summary>
/// Value object for Grade Code
/// </summary>
public sealed class GradeCode : ValueObject
{
    public string Value { get; }

    private GradeCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            throw new ArgumentException("Grade code must be between 1 and 50 characters", nameof(value));

        Value = value;
    }

    public static GradeCode Create(string value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
