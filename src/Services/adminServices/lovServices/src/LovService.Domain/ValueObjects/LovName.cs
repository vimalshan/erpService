namespace LovService.Domain.ValueObjects;

public sealed class LovName : IEquatable<LovName>
{
    public string Value { get; }

    private LovName(string value) => Value = value;

    public static LovName Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value.Length > 30)
            throw new ArgumentException("LovName cannot exceed 30 characters.", nameof(value));
        return new LovName(value.Trim());
    }

    public bool Equals(LovName? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is LovName other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    public override string ToString() => Value;

    public static implicit operator string(LovName name) => name.Value;
}
