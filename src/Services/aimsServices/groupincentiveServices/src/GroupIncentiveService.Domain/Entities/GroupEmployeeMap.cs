using GroupIncentiveService.Domain.Events;
using GroupIncentiveService.Domain.Exceptions;

namespace GroupIncentiveService.Domain.Entities;

public class GroupEmployeeMap : BaseEntity
{
    public long GrpEmpMapId { get; private set; }
    public int GrpEmpMapGroupId { get; private set; }
    public long GrpEmpMapEmpSysId { get; private set; }
    public DateTime GrpEmpMapEffDate { get; private set; }
    public DateTime? GrpEmpMapClsDate { get; private set; }
    public string? GrpEmpMapRole { get; private set; }
    public long GrpEmpMapLastModifiedBy { get; private set; }
    public DateTime GrpEmpMapLastModifiedOn { get; private set; }

    public GroupMaster? Group { get; private set; }

    private GroupEmployeeMap() { }

    public static GroupEmployeeMap Create(long id, int groupId, long employeeId,
        DateTime effDate, string? role, long createdBy)
    {
        if (groupId <= 0)
            throw new DomainException("Invalid group ID.");
        if (employeeId <= 0)
            throw new DomainException("Invalid employee ID.");

        var mapping = new GroupEmployeeMap
        {
            GrpEmpMapId = id,
            GrpEmpMapGroupId = groupId,
            GrpEmpMapEmpSysId = employeeId,
            GrpEmpMapEffDate = effDate,
            GrpEmpMapRole = role?.Trim(),
            GrpEmpMapLastModifiedBy = createdBy,
            GrpEmpMapLastModifiedOn = DateTime.UtcNow
        };

        mapping.AddDomainEvent(new EmployeeAddedToGroupEvent(id, groupId, employeeId, role));
        return mapping;
    }

    public void RemoveFromGroup(DateTime closeDate, long modifiedBy)
    {
        if (GrpEmpMapClsDate.HasValue)
            throw new DomainException("Employee mapping already closed.");

        GrpEmpMapClsDate = closeDate;
        GrpEmpMapLastModifiedBy = modifiedBy;
        GrpEmpMapLastModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive => !GrpEmpMapClsDate.HasValue || GrpEmpMapClsDate > DateTime.UtcNow;
}
