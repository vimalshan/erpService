namespace FeedbackService.Domain.ValueObjects;

/// <summary>
/// Represents the status of feedback (Active/Inactive)
/// </summary>
public class FeedbackStatus : Common.ValueObject
{
    /// <summary>
    /// Initializes a new instance of the FeedbackStatus class
    /// </summary>
    public FeedbackStatus(string? value)
    {
        if (!string.IsNullOrEmpty(value) && value.Length > 1)
            throw new ArgumentException("Status must be a single character", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Gets the status value
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Gets a value indicating whether the status is active
    /// </summary>
    public bool IsActive => Value == "A";

    /// <summary>
    /// Gets a value indicating whether the status is inactive
    /// </summary>
    public bool IsInactive => Value == "I";

    /// <summary>
    /// Creates an active status
    /// </summary>
    public static FeedbackStatus Active() => new("A");

    /// <summary>
    /// Creates an inactive status
    /// </summary>
    public static FeedbackStatus Inactive() => new("I");

    /// <summary>
    /// Gets the atomic values that make up this value object
    /// </summary>
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
