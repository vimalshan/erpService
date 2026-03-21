using FinanceService.Domain.Common;

namespace FinanceService.Domain.Events;

public class BatchApprovedEvent : IDomainEvent
{
    public string UnitCode { get; }
    public decimal BatchNumber { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public BatchApprovedEvent(string unitCode, decimal batchNumber)
    {
        UnitCode = unitCode;
        BatchNumber = batchNumber;
    }
}
