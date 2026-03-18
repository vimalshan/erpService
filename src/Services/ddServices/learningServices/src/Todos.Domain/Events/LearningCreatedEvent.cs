using Todos.Domain.Abstractions;

namespace Todos.Domain.Events;

/// <summary>
/// Raised when a learning & training record is created
/// </summary>
public class LearningCreatedEvent : DomainEvent
{
    public decimal RequestNumber { get; set; }
    public string? EmployeeId { get; set; }
    public string? SpecificNeed { get; set; }
    public DateTime CreatedAt { get; set; }
}
