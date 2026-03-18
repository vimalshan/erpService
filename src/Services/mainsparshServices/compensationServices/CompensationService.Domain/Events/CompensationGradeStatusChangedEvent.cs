using CompensationService.Domain.Common;

namespace CompensationService.Domain.Events;

/// <summary>
/// Event raised when a compensation grade status is changed
/// </summary>
public sealed class CompensationGradeStatusChangedEvent : DomainEvent
{
    public char NewStatus { get; }
    public long ChangedBy { get; }

    public CompensationGradeStatusChangedEvent(
        Guid gradeId,
        char newStatus,
        long changedBy) : base(gradeId)
    {
        NewStatus = newStatus;
        ChangedBy = changedBy;
    }
}
