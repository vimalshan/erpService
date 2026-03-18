using Todos.Domain.Abstractions;

namespace Todos.Domain.ValueObjects;

/// <summary>
/// Value object representing BHR approval status
/// </summary>
public class BHRStatus : ValueObject
{
    /// <summary>
    /// Appraiser can modify
    /// </summary>
    public static readonly BHRStatus CanModify = new('Y');

    /// <summary>
    /// Appraiser cannot modify
    /// </summary>
    public static readonly BHRStatus CannotModify = new('N');

    /// <summary>
    /// Gets the status code
    /// </summary>
    public char Value { get; }

    public BHRStatus(char value)
    {
        if (value != 'Y' && value != 'N')
            throw new ArgumentException($"Invalid BHR status: {value}", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
