namespace EmployeeTransactionsService.Domain.Common;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOnUtc { get; }
}