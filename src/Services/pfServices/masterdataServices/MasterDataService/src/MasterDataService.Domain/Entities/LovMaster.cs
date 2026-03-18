using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class LovMaster : AggregateRoot
{
    public decimal LovId { get; set; }
    public string LovCode { get; set; } = string.Empty;
    public string LovDescription { get; set; } = string.Empty;
    public string LovValue { get; set; } = string.Empty;
    public string LovCategory { get; set; } = string.Empty;
    public string LovStatus { get; set; } = "A";

    public void Activate()
    {
        LovStatus = "A";
        AddDomainEvent(new LovStatusChangedEvent(LovId, "A"));
    }

    public void Deactivate()
    {
        LovStatus = "I";
        AddDomainEvent(new LovStatusChangedEvent(LovId, "I"));
    }
}
