using UserService.Domain.Abstractions;

namespace UserService.Domain.ValueObjects;

/// <summary>
/// Email value object
/// </summary>
public class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!email.Contains("@"))
            throw new ArgumentException("Email is invalid", nameof(email));

        return new Email(email);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

/// <summary>
/// User name value object with validation
/// </summary>
public class UserName : ValueObject
{
    public string Value { get; }

    private UserName(string value)
    {
        Value = value;
    }

    public static UserName Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 100)
            throw new ArgumentException("Name must be between 3 and 100 characters", nameof(name));

        return new UserName(name);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

/// <summary>
/// Password value object (hashed)
/// </summary>
public class PasswordHash : ValueObject
{
    public string Hash { get; }

    private PasswordHash(string hash)
    {
        Hash = hash;
    }

    public static PasswordHash Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            throw new ArgumentException("Password must be at least 6 characters", nameof(password));

        // In production, use BCrypt or PBKDF2
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return new PasswordHash(hash);
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, Hash);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
    }
}

/// <summary>
/// Business Unit ID value object
/// </summary>
public class BusinessUnitId : ValueObject
{
    public string Value { get; }

    private BusinessUnitId(string value)
    {
        Value = value;
    }

    public static BusinessUnitId Create(string buId)
    {
        if (string.IsNullOrWhiteSpace(buId) || buId.Length > 25)
            throw new ArgumentException("Business Unit ID must be between 1 and 25 characters", nameof(buId));

        return new BusinessUnitId(buId);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
