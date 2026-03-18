using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class RateMaster : AggregateRoot
{
    public string TrustCode { get; set; } = string.Empty;
    public int RateId { get; set; }
    public string? RateTypeCode { get; set; }
    public string? RateEffectiveDate { get; set; }
    public string? RateClosingDate { get; set; }
    public decimal? RateValue { get; set; }
    public string? RateDeleteFlag { get; set; }
    public string? ReworkStatus { get; set; }

    public RateType? RateType { get; set; }

    public void UpdateRate(decimal newValue)
    {
        var oldValue = RateValue;
        RateValue = newValue;
        AddDomainEvent(new RateChangedEvent(TrustCode, RateId, oldValue, newValue));
    }
}
