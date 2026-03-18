using Todos.Domain.Abstractions;

namespace Todos.Domain.ValueObjects;

/// <summary>
/// Value object for user/employee identifier
/// </summary>
public class EmployeeId : ValueObject
{
    /// <summary>
    /// Gets the employee identifier
    /// </summary>
    public string Value { get; }

    public EmployeeId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Employee ID cannot be null or empty", nameof(value));

        if (value.Length > 30)
            throw new ArgumentException("Employee ID cannot exceed 30 characters", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
