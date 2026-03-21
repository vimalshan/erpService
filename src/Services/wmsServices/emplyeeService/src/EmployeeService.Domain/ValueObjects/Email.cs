using System.Text.RegularExpressions;
using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.ValueObjects;

public partial class Email : ValueObject
{
    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email? Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        if (email.Length > 100)
            throw new ArgumentException("Email cannot exceed 100 characters.");

        if (!EmailRegex().IsMatch(email))
            throw new ArgumentException("Invalid email format.");

        return new Email(email);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();
}
