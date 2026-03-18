using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Events;

public sealed class RateChangedEvent : IDomainEvent
{
    public string TrustCode { get; }
    public int RateId { get; }
    public decimal? OldValue { get; }
    public decimal? NewValue { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public RateChangedEvent(string trustCode, int rateId, decimal? oldValue, decimal? newValue)
    {
        TrustCode = trustCode;
        RateId = rateId;
        OldValue = oldValue;
        NewValue = newValue;
    }
}
