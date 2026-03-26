using ReferenceDataService.Domain.Common;
using ReferenceDataService.Domain.Events;

namespace ReferenceDataService.Domain.Entities;

public class LovTypeMaster : BaseEntity
{
    public string LovTypeCode { get; private set; } = string.Empty;
    public string? LovTypeName { get; private set; }

    private LovTypeMaster() { }

    public LovTypeMaster(string lovTypeCode, string? lovTypeName)
    {
        LovTypeCode = lovTypeCode ?? throw new ArgumentNullException(nameof(lovTypeCode));
        LovTypeName = lovTypeName;

        AddDomainEvent(new LovTypeMasterCreatedEvent(this));
    }

    public void Update(string? lovTypeName)
    {
        LovTypeName = lovTypeName;

        AddDomainEvent(new LovTypeMasterUpdatedEvent(this));
    }
}
