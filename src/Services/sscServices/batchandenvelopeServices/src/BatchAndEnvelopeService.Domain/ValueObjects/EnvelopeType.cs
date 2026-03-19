using BatchAndEnvelopeService.Domain.Common;

namespace BatchAndEnvelopeService.Domain.ValueObjects;

public sealed class EnvelopeType : ValueObject
{
    public static readonly EnvelopeType Regular = new("REG");
    public static readonly EnvelopeType Express = new("EXP");
    public static readonly EnvelopeType Registered = new("RGD");

    public string Value { get; }

    private EnvelopeType(string value) => Value = value;

    public static EnvelopeType From(string value)
    {
        var type = new EnvelopeType(value.ToUpperInvariant().Trim());
        if (type.Value.Length > 3)
            throw new ArgumentException($"EnvelopeType '{value}' is not valid. Max 3 chars.");
        return type;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
