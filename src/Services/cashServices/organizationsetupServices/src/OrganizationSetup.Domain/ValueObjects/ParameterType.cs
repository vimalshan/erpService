namespace OrganizationSetup.Domain.ValueObjects;

/// <summary>ORG_PARAMTYPE: 6-character code like MAXDEAL, MAXEXP, MINAPP, REPFRQ, FISYEAR, BASECUR.</summary>
public sealed class ParameterType
{
    private static readonly HashSet<string> ValidTypes =
    [
        "MAXDEAL", "MAXEXP", "MINAPP", "REPFRQ", "FISYEAR", "BASECUR"
    ];

    public string Value { get; }

    private ParameterType(string value) => Value = value;

    public static ParameterType Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        var upper = value.Trim().ToUpperInvariant();
        if (upper.Length > 6)
            throw new ArgumentException("Parameter type must not exceed 6 characters.", nameof(value));
        return new ParameterType(upper);
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ParameterType other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);
}
