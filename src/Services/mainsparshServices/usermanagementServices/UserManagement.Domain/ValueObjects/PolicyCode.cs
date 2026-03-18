using UserManagement.Domain.Common;

namespace UserManagement.Domain.ValueObjects;

public sealed class PolicyCode : IEquatable<PolicyCode>
{
    public string Value { get; }

    private PolicyCode(string value) => Value = value;

    public static PolicyCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Policy code cannot be empty.");

        code = code.Trim().ToUpperInvariant();

        if (code.Length > 50)
            throw new DomainException("Policy code cannot exceed 50 characters.");

        if (!System.Text.RegularExpressions.Regex.IsMatch(code, @"^[A-Z0-9_\-]+$"))
            throw new DomainException("Policy code can only contain letters, digits, hyphens and underscores.");

        return new PolicyCode(code);
    }

    public bool Equals(PolicyCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is PolicyCode c && Equals(c);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
