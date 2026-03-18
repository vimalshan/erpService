namespace GSTComplianceService.Domain.ValueObjects;

public sealed class GstinNumber : IEquatable<GstinNumber>
{
    private static readonly System.Text.RegularExpressions.Regex GstinRegex =
        new(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$",
            System.Text.RegularExpressions.RegexOptions.Compiled);

    public string Value { get; }

    private GstinNumber(string value) => Value = value;

    public static GstinNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("GSTIN cannot be empty.");
        var normalized = value.Trim().ToUpperInvariant();
        if (!GstinRegex.IsMatch(normalized))
            throw new ArgumentException($"Invalid GSTIN format: {value}");
        return new GstinNumber(normalized);
    }

    public static GstinNumber? TryCreate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        try { return Create(value); } catch { return null; }
    }

    public bool Equals(GstinNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is GstinNumber g && Equals(g);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
