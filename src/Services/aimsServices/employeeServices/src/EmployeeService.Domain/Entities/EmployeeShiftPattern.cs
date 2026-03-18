using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMPLOYEE_SHIFTPATTERN — monthly shift pattern change record.</summary>
public sealed class EmployeeShiftPattern : BaseAuditableEntity
{
    public long EmpShiftId { get; private set; }
    public long? TimeUnitId { get; private set; }
    public EmployeeId? EmpSysId { get; private set; }
    public int? YearMonth { get; private set; }
    public string? OrgPattern { get; private set; }
    public string? NewPattern { get; private set; }

    private EmployeeShiftPattern() { }

    public static EmployeeShiftPattern Create(long shiftId, long empSysId, long timeUnitId, int yearMonth, string orgPattern, string newPattern, long modifiedBy)
    {
        return new EmployeeShiftPattern
        {
            EmpShiftId = shiftId,
            EmpSysId = EmployeeId.Of(empSysId),
            TimeUnitId = timeUnitId,
            YearMonth = yearMonth,
            OrgPattern = orgPattern,
            NewPattern = newPattern,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
