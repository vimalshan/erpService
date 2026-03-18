using Todos.Domain.Abstractions;

namespace Todos.Domain.ValueObjects;

/// <summary>
/// Value object representing a feedback status
/// </summary>
public class FeedbackStatus : ValueObject
{
    /// <summary>
    /// Feedback satisfied the need
    /// </summary>
    public static readonly FeedbackStatus Yes = new('Y');

    /// <summary>
    /// Feedback did not satisfy the need
    /// </summary>
    public static readonly FeedbackStatus No = new('N');

    /// <summary>
    /// Feedback partially satisfied the need
    /// </summary>
    public static readonly FeedbackStatus Partial = new('P');

    /// <summary>
    /// Gets the status code
    /// </summary>
    public char Value { get; }

    public FeedbackStatus(char value)
    {
        if (value != 'Y' && value != 'N' && value != 'P')
            throw new ArgumentException($"Invalid feedback status: {value}", nameof(value));

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
