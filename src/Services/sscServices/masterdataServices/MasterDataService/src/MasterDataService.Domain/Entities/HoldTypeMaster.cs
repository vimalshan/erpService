using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class HoldTypeMaster : AuditableEntity<long>
{
    public string? HoldName { get; private set; }
    public string? HoldCategory { get; private set; }

    private HoldTypeMaster() { }

    public static HoldTypeMaster Create(long id, string? holdName, string? holdCategory)
    {
        var entity = new HoldTypeMaster
        {
            Id = id,
            HoldName = holdName,
            HoldCategory = holdCategory,
            CreatedAt = DateTime.UtcNow
        };

        entity.AddDomainEvent(new HoldTypeMasterCreatedEvent(entity.Id, entity.HoldName));
        return entity;
    }

    public void Update(string? holdName, string? holdCategory)
    {
        HoldName = holdName;
        HoldCategory = holdCategory;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new HoldTypeMasterUpdatedEvent(Id, HoldName));
    }
}
