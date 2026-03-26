using ReferenceDataService.Domain.Common;
using ReferenceDataService.Domain.Events;

namespace ReferenceDataService.Domain.Entities;

public class LovMaster : BaseEntity
{
    public string LovId { get; private set; } = string.Empty;
    public string? LovType { get; private set; }
    public string? LovName { get; private set; }

    private LovMaster() { }

    public LovMaster(string lovId, string? lovType, string? lovName)
    {
        LovId = lovId ?? throw new ArgumentNullException(nameof(lovId));
        LovType = lovType;
        LovName = lovName;

        AddDomainEvent(new LovMasterCreatedEvent(this));
    }

    public void Update(string? lovType, string? lovName)
    {
        LovType = lovType;
        LovName = lovName;

        AddDomainEvent(new LovMasterUpdatedEvent(this));
    }
}
