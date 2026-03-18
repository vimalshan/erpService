namespace GSTComplianceService.Domain.ValueObjects;

public sealed class PanNumber : IEquatable<PanNumber>
{
    private static readonly System.Text.RegularExpressions.Regex PanRegex =
        new(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", System.Text.RegularExpressions.RegexOptions.Compiled);

    public string Value { get; }

    private PanNumber(string value) => Value = value;

    public static PanNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("PAN number cannot be empty.");
        var normalized = value.Trim().ToUpperInvariant();
        if (!PanRegex.IsMatch(normalized))
            throw new ArgumentException($"Invalid PAN number format: {value}");
        return new PanNumber(normalized);
    }

    public bool Equals(PanNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is PanNumber p && Equals(p);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
