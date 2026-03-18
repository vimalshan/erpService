using Masters.Domain.Common;

namespace Masters.Domain.ValueObjects;

public class LovTypeCode : IEquatable<LovTypeCode>
{
    public string Value { get; }

    private LovTypeCode(string value)
    {
        Value = value;
    }

    public static LovTypeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("LOV Type Code cannot be empty.", nameof(value));
        
        if (value.Length != 3)
            throw new ArgumentException("LOV Type Code must be exactly 3 characters.", nameof(value));

        return new LovTypeCode(value.ToUpperInvariant());
    }

    public bool Equals(LovTypeCode? other)
    {
        if (other is null) return false;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as LovTypeCode);
    public override int GetHashCode() => Value.ToUpperInvariant().GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(LovTypeCode code) => code.Value;
}
