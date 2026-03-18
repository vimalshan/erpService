using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMPLOYEE_APPROVALMAIL.</summary>
public sealed class EmployeeApprovalMail : BaseAuditableEntity
{
    public int AppMailId { get; private set; }
    public EmployeeId EmpSysId { get; private set; } = null!;
    public long AppMailSysId { get; private set; }
    public DateTime EffDate { get; private set; }

    private EmployeeApprovalMail() { }

    public static EmployeeApprovalMail Create(int appMailId, long empSysId, long appMailSysId, long modifiedBy)
    {
        return new EmployeeApprovalMail
        {
            AppMailId = appMailId,
            EmpSysId = EmployeeId.Of(empSysId),
            AppMailSysId = appMailSysId,
            EffDate = DateTime.UtcNow,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
