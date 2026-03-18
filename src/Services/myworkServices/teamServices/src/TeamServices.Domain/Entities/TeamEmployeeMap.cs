using TeamServices.Domain.Common;
using TeamServices.Domain.ValueObjects;

namespace TeamServices.Domain.Entities;

public class TeamEmployeeMap : BaseEntity
{
    public long TeamId { get; private set; }
    public long EmployeeSysId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? CloseDate { get; private set; }

    public TeamMaster? Team { get; private set; }

    private TeamEmployeeMap() { }

    public TeamEmployeeMap(long id, long teamId, long employeeSysId, DateTime effectiveDate, DateTime? closeDate, long modifiedBy)
    {
        Id = id;
        TeamId = teamId;
        EmployeeSysId = employeeSysId;
        EffectiveDate = effectiveDate;
        CloseDate = closeDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;

        if (closeDate.HasValue && closeDate.Value < effectiveDate)
            throw new ArgumentException("Close date cannot be before effective date.");
    }

    public void Close(DateTime closeDate, long modifiedBy)
    {
        if (closeDate < EffectiveDate)
            throw new ArgumentException("Close date cannot be before effective date.");

        CloseDate = closeDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public bool IsActive(DateTime asOfDate)
    {
        return asOfDate >= EffectiveDate && (!CloseDate.HasValue || asOfDate <= CloseDate.Value);
    }
}
