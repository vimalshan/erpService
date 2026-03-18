using EximManagement.Domain.Common;
using EximManagement.Domain.Events;

namespace EximManagement.Domain.Entities;

public class EximProductGroup : BaseEntity
{
    public long GroupId { get; private set; }
    public string GroupName { get; private set; } = default!;
    public long LastUpdatedBy { get; private set; }
    public DateTime LastUpdatedOn { get; private set; }
    public char Status { get; private set; }

    private readonly List<EximProductGroupMap> _mappings = new();
    public IReadOnlyCollection<EximProductGroupMap> Mappings => _mappings.AsReadOnly();

    private EximProductGroup() { }

    public static EximProductGroup Create(long groupId, string groupName, long updatedBy)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new ArgumentException("Group name is required.", nameof(groupName));

        var group = new EximProductGroup
        {
            GroupId = groupId,
            GroupName = groupName,
            LastUpdatedBy = updatedBy,
            LastUpdatedOn = DateTime.UtcNow,
            Status = 'Y'
        };

        group.AddDomainEvent(new EximProductGroupCreatedEvent(group.GroupId, group.GroupName, DateTime.UtcNow));
        return group;
    }

    public void Update(string groupName, long updatedBy)
    {
        GroupName = groupName;
        LastUpdatedBy = updatedBy;
        LastUpdatedOn = DateTime.UtcNow;
    }

    public void AddProduct(EximProductGroupMap mapping) => _mappings.Add(mapping);
}
