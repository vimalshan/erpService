using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Events;

public sealed class LovStatusChangedEvent : IDomainEvent
{
    public decimal LovId { get; }
    public string NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public LovStatusChangedEvent(decimal lovId, string newStatus)
    {
        LovId = lovId;
        NewStatus = newStatus;
    }
}
