namespace UnitService.Domain.ValueObjects;

public class UnitCode : IEquatable<UnitCode>
{
    public string Value { get; }

    private UnitCode(string value) => Value = value;

    public static UnitCode From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 3)
            throw new ArgumentException("Unit code must be 1-3 characters.", nameof(value));

        return new UnitCode(value.Trim().ToUpperInvariant());
    }

    public bool Equals(UnitCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => Equals(obj as UnitCode);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(UnitCode code) => code.Value;
}
