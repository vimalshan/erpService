using GroupIncentiveService.Domain.Exceptions;
using GroupIncentiveService.Domain.Events;

namespace GroupIncentiveService.Domain.Entities;

public class GroupMaster : BaseEntity
{
    public int GroupId { get; private set; }
    public string GroupName { get; private set; } = default!;
    public string? GroupDescription { get; private set; }
    public DateTime GroupEffDate { get; private set; }
    public DateTime? GroupClsDate { get; private set; }
    public string GroupStatus { get; private set; } = "Y";
    public long GroupLastModifiedBy { get; private set; }
    public DateTime GroupLastModifiedOn { get; private set; }

    private readonly List<GroupEmployeeMap> _employeeMappings = [];
    public IReadOnlyCollection<GroupEmployeeMap> EmployeeMappings => _employeeMappings.AsReadOnly();

    private readonly List<GroupIncentiveBreak> _incentiveBreaks = [];
    public IReadOnlyCollection<GroupIncentiveBreak> IncentiveBreaks => _incentiveBreaks.AsReadOnly();

    private GroupMaster() { }

    public static GroupMaster Create(int groupId, string groupName, string? description,
        DateTime effDate, long createdBy)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new DomainException("Group name cannot be empty.");

        var group = new GroupMaster
        {
            GroupId = groupId,
            GroupName = groupName.Trim(),
            GroupDescription = description?.Trim(),
            GroupEffDate = effDate,
            GroupStatus = "Y",
            GroupLastModifiedBy = createdBy,
            GroupLastModifiedOn = DateTime.UtcNow
        };

        group.AddDomainEvent(new GroupCreatedEvent(groupId, groupName, createdBy));
        return group;
    }

    public void Update(string groupName, string? description, long modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new DomainException("Group name cannot be empty.");

        GroupName = groupName.Trim();
        GroupDescription = description?.Trim();
        GroupLastModifiedBy = modifiedBy;
        GroupLastModifiedOn = DateTime.UtcNow;
    }

    public void Close(DateTime closeDate, long modifiedBy)
    {
        if (GroupStatus == "N")
            throw new DomainException("Group is already closed.");

        GroupClsDate = closeDate;
        GroupStatus = "N";
        GroupLastModifiedBy = modifiedBy;
        GroupLastModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive => GroupStatus == "Y";
}
