namespace OrganizationSetup.Domain.ValueObjects;

public sealed class RoleName
{
    public string Value { get; }

    private RoleName(string value) => Value = value;

    public static RoleName Create(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value.Length > 50)
            throw new ArgumentException("Role name must not exceed 50 characters.", nameof(value));
        return new RoleName(value.Trim());
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is RoleName other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode(StringComparison.OrdinalIgnoreCase);
}
