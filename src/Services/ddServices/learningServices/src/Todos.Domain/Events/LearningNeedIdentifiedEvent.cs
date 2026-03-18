using Todos.Domain.Abstractions;

namespace Todos.Domain.Events;

/// <summary>
/// Raised when a learning need is identified
/// </summary>
public class LearningNeedIdentifiedEvent : DomainEvent
{
    public decimal RequestNumber { get; set; }
    public string? DevelopmentArea { get; set; }
    public string? Indicator { get; set; }
    public DateTime IdentifiedAt { get; set; }
}
