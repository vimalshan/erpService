namespace EmployeeTransactionsService.Domain.ValueObjects;

public sealed record EmailAddress(string Value)
{
    public static EmailAddress? CreateOptional(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        var value = email.Trim();
        if (!value.Contains('@', StringComparison.Ordinal))
            throw new ArgumentException("Invalid email address.", nameof(email));

        return new EmailAddress(value);
    }
}