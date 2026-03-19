using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class LovTypeMaster : AuditableEntity<string>
{
    public string LovTypeName { get; private set; } = null!;

    private LovTypeMaster() { }

    public static LovTypeMaster Create(string typeCode, string typeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName);

        var entity = new LovTypeMaster
        {
            Id = typeCode,
            LovTypeName = typeName,
            CreatedAt = DateTime.UtcNow
        };

        entity.AddDomainEvent(new LovTypeMasterCreatedEvent(entity.Id, entity.LovTypeName));
        return entity;
    }

    public void Update(string typeName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(typeName);
        LovTypeName = typeName;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new LovTypeMasterUpdatedEvent(Id, LovTypeName));
    }
}
