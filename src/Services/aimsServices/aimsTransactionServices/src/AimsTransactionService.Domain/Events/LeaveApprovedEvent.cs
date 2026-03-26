using AimsTransactionService.Domain.Common;
using MediatR;

namespace AimsTransactionService.Domain.Events;

public sealed record LeaveApprovedEvent(
    long LeaveDetailId,
    long EmployeeSysId,
    char Status,
    long ApprovedBy) : IDomainEvent, INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
