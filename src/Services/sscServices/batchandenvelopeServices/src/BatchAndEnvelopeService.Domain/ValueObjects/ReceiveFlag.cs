using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.ValueObjects;

public sealed class ReceiveFlag : ValueObject
{
    public static readonly ReceiveFlag Received = new("Y");
    public static readonly ReceiveFlag Pending = new("N");

    public string Value { get; }

    private ReceiveFlag(string value) => Value = value;

    public static ReceiveFlag From(string value)
    {
        var upper = value.ToUpperInvariant().Trim();
        if (upper != "Y" && upper != "N")
            throw new ArgumentException($"ReceiveFlag '{value}' must be 'Y' or 'N'.");
        return new ReceiveFlag(upper);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
