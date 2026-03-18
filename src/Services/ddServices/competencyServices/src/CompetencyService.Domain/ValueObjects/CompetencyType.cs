using CompetencyService.Domain.Common;

namespace CompetencyService.Domain.ValueObjects;

/// <summary>CM_CPD_TYPE: CORE, FUNC, BEHAV, etc.</summary>
public sealed class CompetencyType : ValueObject
{
    public static readonly CompetencyType Core = new("CORE");
    public static readonly CompetencyType Functional = new("FUNC");
    public static readonly CompetencyType Behavioural = new("BEHAV");

    public string Value { get; }

    private CompetencyType(string value) => Value = value;

    public static CompetencyType From(string? value) =>
        value?.ToUpperInvariant() switch
        {
            "CORE" => Core,
            "FUNC" => Functional,
            "BEHAV" => Behavioural,
            null => Core,
            _ => new CompetencyType(value.ToUpperInvariant())
        };

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
