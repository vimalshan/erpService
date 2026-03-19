namespace ApprovalGroup.Domain.ValueObjects;

public sealed class GroupName : IEquatable<GroupName>
{
    public string Value { get; }

    private GroupName(string value) => Value = value;

    public static GroupName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Group name cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Group name cannot exceed 50 characters.", nameof(value));
        return new GroupName(value.Trim());
    }

    public bool Equals(GroupName? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is GroupName other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
    public static implicit operator string(GroupName name) => name.Value;
}
