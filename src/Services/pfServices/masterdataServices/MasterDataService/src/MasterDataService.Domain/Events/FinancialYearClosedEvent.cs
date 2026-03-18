using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Events;

public sealed class FinancialYearClosedEvent : IDomainEvent
{
    public long SerialNumber { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public FinancialYearClosedEvent(long serialNumber)
    {
        SerialNumber = serialNumber;
    }
}
