using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.ValueObjects;

public sealed class SummaryFlag : ValueObject
{
    public static readonly SummaryFlag Yes = new("Y");
    public static readonly SummaryFlag No = new("N");

    public string Value { get; }

    private SummaryFlag(string value) => Value = value;

    public static SummaryFlag From(string value)
    {
        var upper = value.ToUpperInvariant().Trim();
        if (upper != "Y" && upper != "N")
            throw new ArgumentException($"SummaryFlag '{value}' must be 'Y' or 'N'.");
        return new SummaryFlag(upper);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
