using Todos.Domain.Abstractions;

namespace Todos.Domain.Events;

/// <summary>
/// Raised when learning feedback is submitted
/// </summary>
public class FeedbackSubmittedEvent : DomainEvent
{
    public decimal RequestNumber { get; set; }
    public char FeedbackStatus { get; set; }
    public DateTime SubmittedAt { get; set; }
}
