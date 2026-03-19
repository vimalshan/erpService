namespace SecurityService.Domain.ValueObjects;

public sealed class UserCode : IEquatable<UserCode>
{
    public string Value { get; }

    private UserCode(string value) => Value = value;

    public static UserCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 25)
            throw new Exceptions.InvalidUserCodeException(value ?? string.Empty);
        return new UserCode(value.Trim());
    }

    public bool Equals(UserCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is UserCode other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(UserCode code) => code.Value;
}
