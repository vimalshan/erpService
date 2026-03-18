using TdsService.Domain.Common;
using TdsService.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace TdsService.Domain.ValueObjects;

/// <summary>
/// Indian PAN (Permanent Account Number) value object.
/// Format: AAAAA0000A — 5 letters, 4 digits, 1 letter.
/// </summary>
public sealed class PanNumber : ValueObject
{
    private static readonly Regex PanPattern = new(@"^[A-Z]{5}[0-9]{4}[A-Z]$", RegexOptions.Compiled);

    public string Value { get; }

    private PanNumber(string value) => Value = value;

    public static PanNumber Create(string? pan)
    {
        if (string.IsNullOrWhiteSpace(pan))
            throw new InvalidPanNumberException("PAN number cannot be empty.");

        var normalised = pan.Trim().ToUpperInvariant();

        if (!PanPattern.IsMatch(normalised))
            throw new InvalidPanNumberException($"'{pan}' is not a valid PAN number. Expected format: AAAAA0000A.");

        return new PanNumber(normalised);
    }

    public static PanNumber? TryCreate(string? pan)
    {
        try { return Create(pan); }
        catch { return null; }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
