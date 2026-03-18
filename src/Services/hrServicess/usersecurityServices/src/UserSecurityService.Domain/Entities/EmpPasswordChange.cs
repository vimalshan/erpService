using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Records each time an employee's password is changed (audit trail).</summary>
public class EmpPasswordChange : BaseEntity
{
    public decimal EpwdId { get; private set; }
    public decimal EpwdEmpSysId { get; private set; }
    public decimal EpwdCreatedBy { get; private set; }
    public DateTime EpwdCreatedOn { get; private set; }

    private EmpPasswordChange() { }

    public static EmpPasswordChange Create(decimal id, decimal empSysId, decimal createdBy)
    {
        return new EmpPasswordChange
        {
            EpwdId = id,
            EpwdEmpSysId = empSysId,
            EpwdCreatedBy = createdBy,
            EpwdCreatedOn = DateTime.UtcNow
        };
    }
}
