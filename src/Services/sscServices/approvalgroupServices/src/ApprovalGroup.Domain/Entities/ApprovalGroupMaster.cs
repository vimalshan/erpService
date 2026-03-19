using ApprovalGroup.Domain.Events;

namespace ApprovalGroup.Domain.Entities;

/// <summary>
/// Maps to APGROUP_MAST table - Approval Group Master
/// </summary>
public class ApprovalGroupMaster : BaseEntity
{
    public long GroupId { get; private set; }
    public string GroupName { get; private set; } = string.Empty;
    public long GroupCreatedBy { get; private set; }
    public DateTime GroupCreatedOn { get; private set; }
    public long? GroupModifiedBy { get; private set; }
    public DateTime? GroupModifiedOn { get; private set; }
    public long? GroupPriorityId { get; private set; }

    // Navigation
    public ICollection<ApprovalGroupMap> GroupMaps { get; private set; } = new List<ApprovalGroupMap>();
    public ICollection<ApprovalGroupUserMap> UserMaps { get; private set; } = new List<ApprovalGroupUserMap>();

    private ApprovalGroupMaster() { }

    public static ApprovalGroupMaster Create(long groupId, string groupName, long createdBy, long? priorityId = null)
    {
        var group = new ApprovalGroupMaster
        {
            GroupId = groupId,
            GroupName = groupName,
            GroupCreatedBy = createdBy,
            GroupCreatedOn = DateTime.UtcNow,
            GroupPriorityId = priorityId
        };
        group.RaiseDomainEvent(new ApprovalGroupCreatedEvent(group.GroupId, group.GroupName));
        return group;
    }

    public void Update(string groupName, long modifiedBy, long? priorityId = null)
    {
        GroupName = groupName;
        GroupModifiedBy = modifiedBy;
        GroupModifiedOn = DateTime.UtcNow;
        GroupPriorityId = priorityId;
        RaiseDomainEvent(new ApprovalGroupUpdatedEvent(GroupId, GroupName));
    }
}
