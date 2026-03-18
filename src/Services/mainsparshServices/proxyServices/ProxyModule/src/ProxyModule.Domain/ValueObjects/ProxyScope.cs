namespace ProxyModule.Domain.ValueObjects;

public sealed class ProxyScope
{
    public static readonly ProxyScope All = new("ALL");
    public static readonly ProxyScope Department = new("DEPARTMENT");
    public static readonly ProxyScope Location = new("LOCATION");
    public static readonly ProxyScope Specific = new("SPECIFIC");

    public string Value { get; }

    private ProxyScope(string value) => Value = value;

    public static ProxyScope From(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return All;

        return value.ToUpperInvariant() switch
        {
            "ALL" => All,
            "DEPARTMENT" => Department,
            "LOCATION" => Location,
            "SPECIFIC" => Specific,
            _ => throw new ArgumentException($"Invalid proxy scope: {value}")
        };
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ProxyScope other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
