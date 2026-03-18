using Todos.Domain.Abstractions;

namespace Todos.Domain.Events;

/// <summary>
/// Raised when a learning & training record is updated
/// </summary>
public class LearningUpdatedEvent : DomainEvent
{
    public decimal RequestNumber { get; set; }
    public string? EmployeeId { get; set; }
    public DateTime UpdatedAt { get; set; }
}
