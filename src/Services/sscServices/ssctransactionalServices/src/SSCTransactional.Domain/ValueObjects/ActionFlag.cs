using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.ValueObjects;

/// <summary>
/// Action flag for allocation: N=Awaiting, H=Hold, D=Defect, F=Forward, C=Completed, P=Self-Hold, R=Hold Released, S=Rescan, E=Rejected, B=Sent Back
/// </summary>
public sealed class ActionFlag : ValueObject
{
    public static readonly ActionFlag Awaiting = new("N");
    public static readonly ActionFlag Hold = new("H");
    public static readonly ActionFlag Defect = new("D");
    public static readonly ActionFlag Forward = new("F");
    public static readonly ActionFlag Completed = new("C");
    public static readonly ActionFlag SelfHold = new("P");
    public static readonly ActionFlag HoldReleased = new("R");
    public static readonly ActionFlag Rescan = new("S");
    public static readonly ActionFlag Rejected = new("E");
    public static readonly ActionFlag SentBack = new("B");

    public string Value { get; }

    private ActionFlag(string value) => Value = value;

    public static ActionFlag From(string value)
    {
        return value.ToUpperInvariant() switch
        {
            "N" => Awaiting,
            "H" => Hold,
            "D" => Defect,
            "F" => Forward,
            "C" => Completed,
            "P" => SelfHold,
            "R" => HoldReleased,
            "S" => Rescan,
            "E" => Rejected,
            "B" => SentBack,
            _ => throw new ArgumentException($"Invalid action flag: {value}")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
