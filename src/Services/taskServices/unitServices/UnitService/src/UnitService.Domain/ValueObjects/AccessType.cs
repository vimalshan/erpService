namespace UnitService.Domain.ValueObjects;

public class AccessType : IEquatable<AccessType>
{
    public static readonly AccessType Read = new("R");
    public static readonly AccessType Write = new("W");
    public static readonly AccessType Admin = new("A");

    public string Value { get; }

    private AccessType(string value) => Value = value;

    public static AccessType From(string value)
    {
        return value?.Trim().ToUpperInvariant() switch
        {
            "R" => Read,
            "W" => Write,
            "A" => Admin,
            _ => throw new ArgumentException($"Invalid access type: {value}. Valid values are R, W, A.", nameof(value))
        };
    }

    public bool Equals(AccessType? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as AccessType);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(AccessType type) => type.Value;
}
