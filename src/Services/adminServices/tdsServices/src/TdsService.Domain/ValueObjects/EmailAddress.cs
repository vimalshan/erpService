using TdsService.Domain.Common;
using TdsService.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace TdsService.Domain.ValueObjects;

/// <summary>
/// Email address value object with basic RFC-5322 validation.
/// </summary>
public sealed class EmailAddress : ValueObject
{
    private static readonly Regex EmailPattern = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email address cannot be empty.");

        var normalised = email.Trim();

        if (normalised.Length > 3000)
            throw new DomainException("Email address exceeds maximum length of 3000 characters.");

        if (!EmailPattern.IsMatch(normalised))
            throw new DomainException($"'{email}' is not a valid email address.");

        return new EmailAddress(normalised);
    }

    public static EmailAddress? TryCreate(string? email)
    {
        try { return Create(email); }
        catch { return null; }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value.ToLowerInvariant();
    }

    public override string ToString() => Value;
}
