namespace LoanTransaction.Domain.ValueObjects;

/// <summary>SET=Settled; WOF=Written Off; ADJ=Adjusted; LIV=Living</summary>
public sealed class ClosureType : IEquatable<ClosureType>
{
    public const string Settled = "SET";
    public const string WrittenOff = "WOF";
    public const string Adjusted = "ADJ";
    public const string Living = "LIV";

    public string Value { get; }

    private ClosureType(string value) => Value = value;

    public static ClosureType FromValue(string value)
    {
        var valid = new[] { Settled, WrittenOff, Adjusted, Living };
        if (!valid.Contains(value))
            throw new ArgumentException($"Invalid closure type: {value}", nameof(value));
        return new(value);
    }

    public bool IsSettled => Value == Settled;
    public bool IsWrittenOff => Value == WrittenOff;
    public bool IsAdjusted => Value == Adjusted;
    public bool IsLiving => Value == Living;

    public override bool Equals(object? obj) => Equals(obj as ClosureType);
    public bool Equals(ClosureType? other) => other is not null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
