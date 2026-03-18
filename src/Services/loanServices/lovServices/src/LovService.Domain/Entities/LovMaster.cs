using LovService.Domain.Common;
using LovService.Domain.Events;

namespace LovService.Domain.Entities;

/// <summary>
/// LOV_MASTER - List of Values Master
/// </summary>
public class LovMaster : BaseEntity
{
    public long LovId { get; private set; }
    public int LovTypeId { get; private set; }
    public string LovName { get; private set; } = string.Empty;
    public DateTime LovCreatedOn { get; private set; }
    public long LovCreatedBy { get; private set; }
    public long LovUpdatedBy { get; private set; }
    public DateTime LovUpdatedOn { get; private set; }

    public LovTypeMast? LovType { get; private set; }

    private LovMaster() { }

    public static LovMaster Create(long lovId, int lovTypeId, string lovName, long createdBy)
    {
        var now = DateTime.UtcNow;
        var entity = new LovMaster
        {
            LovId = lovId,
            LovTypeId = lovTypeId,
            LovName = lovName,
            LovCreatedOn = now,
            LovCreatedBy = createdBy,
            LovUpdatedBy = createdBy,
            LovUpdatedOn = now
        };
        entity.AddDomainEvent(new LovMasterCreatedEvent(entity));
        return entity;
    }

    public void Update(string lovName, long updatedBy)
    {
        LovName = lovName;
        LovUpdatedBy = updatedBy;
        LovUpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new LovMasterUpdatedEvent(this));
    }

    public void Delete()
        => AddDomainEvent(new LovMasterDeletedEvent(LovId, LovTypeId));
}
