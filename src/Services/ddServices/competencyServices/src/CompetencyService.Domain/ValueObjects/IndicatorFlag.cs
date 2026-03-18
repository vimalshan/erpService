using CompetencyService.Domain.Common;
using CompetencyService.Domain.Exceptions;

namespace CompetencyService.Domain.ValueObjects;

/// <summary>IND_FLAG: P (Positive) or N (Negative)</summary>
public sealed class IndicatorFlag : ValueObject
{
    public static readonly IndicatorFlag Positive = new('P');
    public static readonly IndicatorFlag Negative = new('N');

    public char Value { get; }

    private IndicatorFlag(char value) => Value = value;

    public static IndicatorFlag From(char? value) =>
        value?.ToString().ToUpperInvariant()[0] switch
        {
            'P' => Positive,
            'N' => Negative,
            null => Positive,
            _ => throw new CompetencyDomainException($"Invalid IndicatorFlag value: {value}")
        };

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
