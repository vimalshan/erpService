using AttendanceService.Domain.Common;

namespace AttendanceService.Domain.Events;

public sealed record AttendanceBatchProcessedEvent(
    long BatchId,
    int Month,
    int Year) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
