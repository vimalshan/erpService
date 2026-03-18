namespace AuditService.Domain.ValueObjects;

/// <summary>
/// Value object representing a risk rating for an audit observation.
/// </summary>
public sealed class RiskRating : IEquatable<RiskRating>
{
    private static readonly IReadOnlyDictionary<char, string> _descriptions = new Dictionary<char, string>
    {
        { 'A', "High Risk" },
        { 'B', "Medium-High Risk" },
        { 'C', "Medium Risk" },
        { 'D', "Low Risk" }
    };

    public char Code { get; }
    public string Description => _descriptions[Code];
    public int NumericValue => Code - 'A' + 1;

    private RiskRating(char code) => Code = code;

    public static RiskRating Create(char code)
    {
        code = char.ToUpper(code);
        if (!_descriptions.ContainsKey(code))
            throw new ArgumentException($"Invalid risk code '{code}'. Must be A, B, C, or D.");
        return new RiskRating(code);
    }

    public static RiskRating High => new('A');
    public static RiskRating MediumHigh => new('B');
    public static RiskRating Medium => new('C');
    public static RiskRating Low => new('D');

    public bool Equals(RiskRating? other) => other is not null && Code == other.Code;
    public override bool Equals(object? obj) => obj is RiskRating other && Equals(other);
    public override int GetHashCode() => Code.GetHashCode();
    public override string ToString() => $"{Code} - {Description}";
}
