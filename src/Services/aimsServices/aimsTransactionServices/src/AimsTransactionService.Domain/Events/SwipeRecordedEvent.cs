using AimsTransactionService.Domain.Common;
using MediatR;

namespace AimsTransactionService.Domain.Events;

public sealed record SwipeRecordedEvent(
    long SwipeId,
    long EmployeeSysId,
    DateTime PunchTime,
    char PunchStatus) : IDomainEvent, INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
