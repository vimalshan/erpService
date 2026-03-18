namespace ProxyModule.Domain.ValueObjects;

public sealed class ProxyType
{
    public static readonly ProxyType Approval = new("APPROVAL");
    public static readonly ProxyType Submission = new("SUBMISSION");
    public static readonly ProxyType Full = new("FULL");
    public static readonly ProxyType ReadOnly = new("READONLY");

    public string Value { get; }

    private ProxyType(string value) => Value = value;

    public static ProxyType From(string value)
    {
        return value?.ToUpperInvariant() switch
        {
            "APPROVAL" => Approval,
            "SUBMISSION" => Submission,
            "FULL" => Full,
            "READONLY" => ReadOnly,
            _ => throw new ArgumentException($"Invalid proxy type: {value}")
        };
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ProxyType other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
