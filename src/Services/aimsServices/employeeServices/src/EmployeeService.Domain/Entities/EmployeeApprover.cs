using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMPLOYEE_APPROVER.</summary>
public sealed class EmployeeApprover : BaseAuditableEntity
{
    public int ApproverId { get; private set; }
    public EmployeeId EmpSysId { get; private set; } = null!;
    public ApproverLevel Level { get; private set; } = null!;
    public long ApproverSysId { get; private set; }
    public DateTime EffDate { get; private set; }

    private EmployeeApprover() { }

    public static EmployeeApprover Create(int approverId, long empSysId, int level, long approverSysId, long assignedBy)
    {
        var entity = new EmployeeApprover
        {
            ApproverId = approverId,
            EmpSysId = EmployeeId.Of(empSysId),
            Level = ApproverLevel.Of(level),
            ApproverSysId = approverSysId,
            EffDate = DateTime.UtcNow,
            LastModifiedBy = assignedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.ApproverAssignedEvent(approverId, empSysId, approverSysId, level));
        return entity;
    }
}
