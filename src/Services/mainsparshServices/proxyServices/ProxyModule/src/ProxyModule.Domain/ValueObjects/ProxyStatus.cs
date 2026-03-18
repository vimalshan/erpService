namespace ProxyModule.Domain.ValueObjects;

public sealed class ProxyStatus
{
    public static readonly ProxyStatus Active = new("A");
    public static readonly ProxyStatus Inactive = new("I");

    public string Value { get; }

    private ProxyStatus(string value) => Value = value;

    public static ProxyStatus From(string? value)
    {
        return value?.ToUpperInvariant() switch
        {
            "A" or null => Active,
            "I" => Inactive,
            _ => throw new ArgumentException($"Invalid proxy status: {value}")
        };
    }

    public bool IsActive => Value == "A";

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ProxyStatus other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
