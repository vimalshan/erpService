namespace ApprovalGroup.Domain.ValueObjects;

public sealed class BusinessUnitId : IEquatable<BusinessUnitId>
{
    public string Value { get; }

    private BusinessUnitId(string value) => Value = value;

    public static BusinessUnitId Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Business unit ID cannot be empty.", nameof(value));
        if (value.Length > 25)
            throw new ArgumentException("Business unit ID cannot exceed 25 characters.", nameof(value));
        return new BusinessUnitId(value.Trim());
    }

    public bool Equals(BusinessUnitId? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is BusinessUnitId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
    public static implicit operator string(BusinessUnitId id) => id.Value;
}
