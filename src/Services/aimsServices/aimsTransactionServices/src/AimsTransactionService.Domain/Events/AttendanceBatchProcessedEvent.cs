using AimsTransactionService.Domain.Common;
using MediatR;

namespace AimsTransactionService.Domain.Events;

public sealed record AttendanceBatchProcessedEvent(
    long BatchId,
    DateTime MonthStart,
    DateTime MonthEnd,
    int EmployeesProcessed) : IDomainEvent, INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
