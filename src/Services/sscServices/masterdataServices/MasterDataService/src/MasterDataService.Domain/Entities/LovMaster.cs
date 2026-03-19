using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class LovMaster : AuditableEntity<long>
{
    public string LovType { get; private set; } = null!;
    public string LovName { get; private set; } = null!;

    private LovMaster() { }

    public static LovMaster Create(long id, string lovType, string lovName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lovType);
        ArgumentException.ThrowIfNullOrWhiteSpace(lovName);

        var entity = new LovMaster
        {
            Id = id,
            LovType = lovType,
            LovName = lovName,
            CreatedAt = DateTime.UtcNow
        };

        entity.AddDomainEvent(new LovMasterCreatedEvent(entity.Id, entity.LovType, entity.LovName));
        return entity;
    }

    public void Update(string lovType, string lovName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lovType);
        ArgumentException.ThrowIfNullOrWhiteSpace(lovName);

        LovType = lovType;
        LovName = lovName;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new LovMasterUpdatedEvent(Id, LovType, LovName));
    }
}
