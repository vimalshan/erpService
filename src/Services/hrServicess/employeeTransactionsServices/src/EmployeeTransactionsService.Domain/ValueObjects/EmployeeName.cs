namespace EmployeeTransactionsService.Domain.ValueObjects;

public sealed record EmployeeName(string FirstName, string? MiddleName, string? LastName)
{
    public string FullName => string.Join(" ", new[] { FirstName, MiddleName, LastName }
        .Where(static part => !string.IsNullOrWhiteSpace(part)));

    public static EmployeeName Create(string firstName, string? middleName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));

        return new EmployeeName(firstName.Trim(), middleName?.Trim(), lastName?.Trim());
    }
}