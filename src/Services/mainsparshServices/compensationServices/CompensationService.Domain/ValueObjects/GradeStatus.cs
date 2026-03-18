using CompensationService.Domain.Common;

namespace CompensationService.Domain.ValueObjects;

/// <summary>
/// Value object for Grade Status
/// </summary>
public sealed class GradeStatus : ValueObject
{
    public const char Active = 'A';
    public const char Inactive = 'I';

    public char Value { get; }

    private GradeStatus(char value)
    {
        if (value != Active && value != Inactive)
            throw new ArgumentException("Grade status must be 'A' (Active) or 'I' (Inactive)", nameof(value));

        Value = value;
    }

    public static GradeStatus CreateActive() => new(Active);
    public static GradeStatus CreateInactive() => new(Inactive);
    public static GradeStatus Create(char value) => new(value);

    public bool IsActive => Value == Active;
    public bool IsInactive => Value == Inactive;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value == Active ? "Active" : "Inactive";
}
