using AimsTransactionService.Domain.Common;
using MediatR;

namespace AimsTransactionService.Domain.Events;

public sealed record LeaveAppliedEvent(
    long LeaveDetailId,
    long EmployeeSysId,
    long LeaveId,
    DateTime FromDate,
    DateTime ToDate,
    decimal LeaveDays) : IDomainEvent, INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
