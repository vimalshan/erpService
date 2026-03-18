using GroupManagementService.Domain.ValueObjects;

namespace GroupManagementService.Domain.Events
{
    /// <summary>
    /// Base class for domain events
    /// </summary>
    public abstract class DomainEvent
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredAt { get; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Event raised when a group is created
    /// </summary>
    public class GroupCreatedEvent : DomainEvent
    {
        public long GroupId { get; }
        public string GroupCode { get; }
        public string GroupName { get; }

        public GroupCreatedEvent(long groupId, string groupCode, string groupName)
        {
            GroupId = groupId;
            GroupCode = groupCode;
            GroupName = groupName;
        }
    }

    /// <summary>
    /// Event raised when a group is updated
    /// </summary>
    public class GroupUpdatedEvent : DomainEvent
    {
        public long GroupId { get; }
        public string GroupName { get; }
        public string? Description { get; }

        public GroupUpdatedEvent(long groupId, string groupName, string? description)
        {
            GroupId = groupId;
            GroupName = groupName;
            Description = description;
        }
    }

    /// <summary>
    /// Event raised when group status changes
    /// </summary>
    public class GroupStatusChangedEvent : DomainEvent
    {
        public long GroupId { get; }
        public GroupStatus NewStatus { get; }

        public GroupStatusChangedEvent(long groupId, GroupStatus newStatus)
        {
            GroupId = groupId;
            NewStatus = newStatus;
        }
    }

    /// <summary>
    /// Event raised when a menu is added to a group
    /// </summary>
    public class MenuMapAddedEvent : DomainEvent
    {
        public long GroupId { get; }
        public string MenuCode { get; }

        public MenuMapAddedEvent(long groupId, string menuCode)
        {
            GroupId = groupId;
            MenuCode = menuCode;
        }
    }

    /// <summary>
    /// Event raised when a menu is removed from a group
    /// </summary>
    public class MenuMapRemovedEvent : DomainEvent
    {
        public long GroupId { get; }
        public string MenuCode { get; }

        public MenuMapRemovedEvent(long groupId, string menuCode)
        {
            GroupId = groupId;
            MenuCode = menuCode;
        }
    }

    /// <summary>
    /// Event raised when menu permissions are updated
    /// </summary>
    public class MenuPermissionsUpdatedEvent : DomainEvent
    {
        public long GroupId { get; }
        public string MenuCode { get; }
        public MenuPermissions Permissions { get; }

        public MenuPermissionsUpdatedEvent(long groupId, string menuCode, MenuPermissions permissions)
        {
            GroupId = groupId;
            MenuCode = menuCode;
            Permissions = permissions;
        }
    }
}
