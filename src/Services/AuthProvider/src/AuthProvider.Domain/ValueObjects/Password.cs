using AuthProvider.Domain.Common;

namespace AuthProvider.Domain.ValueObjects;

/// <summary>
/// Password value object. Stores a BCrypt hash, never plain-text.
/// Verification is delegated to IPasswordHasher in the Application layer.
/// </summary>
public sealed class Password : ValueObject
{
    public string Hash { get; }

    private Password(string hash) => Hash = hash;

    /// <summary>Create from a pre-computed hash (supplied by the infrastructure hasher).</summary>
    public static Password FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(hash));
        return new Password(hash);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Hash;
    }

    public override string ToString() => "***";
}
