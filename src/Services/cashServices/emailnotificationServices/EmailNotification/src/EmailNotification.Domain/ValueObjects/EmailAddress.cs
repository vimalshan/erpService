namespace EmailNotification.Domain.ValueObjects;

/// <summary>
/// Email address value object
/// </summary>
public class EmailAddress : Common.ValueObject
{
    /// <summary>
    /// The email address value
    /// </summary>
    public string Value { get; private set; }

    /// <summary>
    /// Private parameterless constructor for Entity Framework Core only
    /// </summary>
    private EmailAddress() { }

    /// <summary>
    /// Initializes a new instance of the EmailAddress class
    /// </summary>
    /// <param name="value">The email address value</param>
    /// <exception cref="ArgumentException">Thrown when the email address is invalid</exception>
    public EmailAddress(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email address cannot be empty", nameof(value));

        if (!IsValidEmail(value))
            throw new ArgumentException("Email address format is invalid", nameof(value));

        Value = value.ToLowerInvariant();
    }

    /// <summary>
    /// Validates the email address format
    /// </summary>
    /// <param name="email">The email address to validate</param>
    /// <returns>true if the email address is valid; otherwise, false</returns>
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Gets the components for equality comparison
    /// </summary>
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <summary>
    /// Returns the email address value
    /// </summary>
    public override string ToString() => Value;
}
