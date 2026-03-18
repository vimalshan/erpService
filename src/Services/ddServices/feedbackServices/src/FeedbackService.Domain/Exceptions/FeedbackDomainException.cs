namespace FeedbackService.Domain.Exceptions;

/// <summary>
/// Thrown when a feedback domain rule is violated
/// </summary>
public class FeedbackDomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the FeedbackDomainException class
    /// </summary>
    public FeedbackDomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the FeedbackDomainException class with a specified error message and a reference to the inner exception
    /// </summary>
    public FeedbackDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
