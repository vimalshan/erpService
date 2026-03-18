using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMPLOYEE_PATTERN — shift pattern assignment.</summary>
public sealed class EmployeePattern : BaseAuditableEntity
{
    public long EmpPatternId { get; private set; }
    public EmployeeId EmpSysId { get; private set; } = null!;
    public int PatternMastId { get; private set; }
    public DateTime EffDate { get; private set; }
    public DateTime? ClsDate { get; private set; }
    public int WeeklyOffDay { get; private set; }
    public int? SubWeeklyDay { get; private set; }
    public string? SubFrequency { get; private set; }

    private EmployeePattern() { }

    public static EmployeePattern Create(long empPatternId, long empSysId, int mastId, int weeklyOffDay, long modifiedBy)
    {
        return new EmployeePattern
        {
            EmpPatternId = empPatternId,
            EmpSysId = EmployeeId.Of(empSysId),
            PatternMastId = mastId,
            EffDate = DateTime.UtcNow,
            WeeklyOffDay = weeklyOffDay,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }
}
