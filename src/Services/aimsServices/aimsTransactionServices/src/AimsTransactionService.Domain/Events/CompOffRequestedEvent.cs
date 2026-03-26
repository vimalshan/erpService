using AimsTransactionService.Domain.Common;
using MediatR;

namespace AimsTransactionService.Domain.Events;

public sealed record CompOffRequestedEvent(
    long CompOffId,
    long EmployeeSysId,
    decimal RequestedHours) : IDomainEvent, INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
