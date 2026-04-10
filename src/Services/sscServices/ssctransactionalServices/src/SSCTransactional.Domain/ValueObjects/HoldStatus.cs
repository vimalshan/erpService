using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.ValueObjects;

/// <summary>
/// Hold/Release status: H=Hold, R=Released
/// </summary>
public sealed class HoldStatus : ValueObject
{
    public static readonly HoldStatus Hold = new("H");
    public static readonly HoldStatus Released = new("R");

    public string Value { get; }

    private HoldStatus(string value) => Value = value;

    public static HoldStatus From(string value)
    {
        return value.ToUpperInvariant() switch
        {
            "H" => Hold,
            "R" => Released,
            _ => throw new ArgumentException($"Invalid hold status: {value}. Must be H or R.")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
