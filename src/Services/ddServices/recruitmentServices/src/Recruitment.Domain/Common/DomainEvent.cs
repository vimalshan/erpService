namespace Recruitment.Domain.Common;

/// <summary>
/// Domain event base class
/// </summary>
public abstract class DomainEvent
{
    public DateTime DateTimeOccurred { get; protected set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
}
