namespace CompensationBenefits.Domain.ValueObjects;

/// <summary>Salary type: C = CTC Based, F = Fixed</summary>
public sealed class SalaryType
{
    public static readonly SalaryType CtcBased = new("C");
    public static readonly SalaryType Fixed = new("F");

    public string Value { get; }

    private SalaryType(string value) => Value = value;

    public static SalaryType From(string value) => value switch
    {
        "C" => CtcBased,
        "F" => Fixed,
        _ => throw new ArgumentException($"Invalid SalaryType: {value}")
    };

    public override string ToString() => Value;
}

/// <summary>Frequency: M = Monthly, A = Annual, Q = Quarterly, H = Half-Yearly</summary>
public sealed class FrequencyType
{
    public static readonly FrequencyType Monthly = new("M");
    public static readonly FrequencyType Annual = new("A");
    public static readonly FrequencyType Quarterly = new("Q");
    public static readonly FrequencyType HalfYearly = new("H");

    public string Value { get; }
    private FrequencyType(string value) => Value = value;

    public static FrequencyType From(string value) => value switch
    {
        "M" => Monthly,
        "A" => Annual,
        "Q" => Quarterly,
        "H" => HalfYearly,
        _ => throw new ArgumentException($"Invalid FrequencyType: {value}")
    };
}
