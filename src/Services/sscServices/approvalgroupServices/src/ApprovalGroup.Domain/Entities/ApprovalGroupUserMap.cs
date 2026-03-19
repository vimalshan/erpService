using ApprovalGroup.Domain.Events;

namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to APGROUP_USERMAP table - AP Group User Mapping
/// </summary>
public class ApprovalGroupUserMap : BaseEntity
{
    public long MapId { get; private set; }
    public long MapGroupId { get; private set; }
    public long MapUserId { get; private set; }
    public DateTime MapEffectiveDate { get; private set; }
    public DateTime? MapClosureDate { get; private set; }
    public long MapCreatedBy { get; private set; }
    public DateTime MapCreatedOn { get; private set; }
    public long? MapModifiedBy { get; private set; }
    public DateTime? MapModifiedOn { get; private set; }

    // Navigation
    public ApprovalGroupMaster? ApprovalGroup { get; private set; }

    private ApprovalGroupUserMap() { }

    public static ApprovalGroupUserMap Create(long mapId, long groupId, long userId,
        DateTime effectiveDate, long createdBy)
    {
        var userMap = new ApprovalGroupUserMap
        {
            MapId = mapId,
            MapGroupId = groupId,
            MapUserId = userId,
            MapEffectiveDate = effectiveDate,
            MapCreatedBy = createdBy,
            MapCreatedOn = DateTime.UtcNow
        };
        userMap.RaiseDomainEvent(new UserMappedToGroupEvent(groupId, userId));
        return userMap;
    }

    public void Close(long modifiedBy)
    {
        MapClosureDate = DateTime.UtcNow;
        MapModifiedBy = modifiedBy;
        MapModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new UserRemovedFromGroupEvent(MapGroupId, MapUserId));
    }
}
