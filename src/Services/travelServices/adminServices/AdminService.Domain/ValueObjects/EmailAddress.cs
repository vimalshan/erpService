using System.ComponentModel.DataAnnotations;

namespace AdminService.Domain.ValueObjects;

/// <summary>
/// Represents an email address value object
/// </summary>
public record EmailAddress
{
    /// <summary>
    /// Email address value
    /// </summary>
    public string Value { get; init; }

    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        if (!new EmailAddressAttribute().IsValid(value))
            throw new ArgumentException("Invalid email format", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}
