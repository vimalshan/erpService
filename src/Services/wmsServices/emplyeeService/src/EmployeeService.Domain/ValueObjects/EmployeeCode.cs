using System.Text.RegularExpressions;
using EmployeeService.Domain.Common;

namespace EmployeeService.Domain.ValueObjects;

public partial class EmployeeCode : ValueObject
{
    public string Value { get; }

    private EmployeeCode(string value) => Value = value;

    public static EmployeeCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Employee code cannot be empty.");

        if (code.Length > 20)
            throw new ArgumentException("Employee code cannot exceed 20 characters.");

        if (!EmployeeCodeRegex().IsMatch(code))
            throw new ArgumentException("Employee code must be alphanumeric with optional hyphens.");

        return new EmployeeCode(code);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    [GeneratedRegex(@"^[A-Za-z0-9\-]+$")]
    private static partial Regex EmployeeCodeRegex();
}
