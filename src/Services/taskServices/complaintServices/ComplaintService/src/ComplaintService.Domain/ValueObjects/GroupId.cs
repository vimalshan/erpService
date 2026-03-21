namespace ComplaintService.Domain.ValueObjects;

public sealed record GroupId
{
    public string Value { get; }

    public GroupId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Group ID cannot be empty.", nameof(value));
        if (value.Length > 255)
            throw new ArgumentException("Group ID cannot exceed 255 characters.", nameof(value));
        Value = value.Trim();
    }

    public static implicit operator string(GroupId g) => g.Value;
    public override string ToString() => Value;
}
