using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMPLOYEE_SHIFT — daily shift schedules per employee.</summary>
public sealed class EmployeeShift : BaseEntity
{
    public long EmpShiftId { get; private set; }
    public EmployeeId EmpSysId { get; private set; } = null!;
    public int TimeUnitId { get; private set; }
    public char ShiftCode { get; private set; }
    public int YearMonth { get; private set; }
    public int Day { get; private set; }
    public DateTime ShiftDate { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private EmployeeShift() { }

    public static EmployeeShift Create(long shiftId, long empSysId, int timeUnitId, char code, int yearMonth, int day, DateTime shiftDate, long updatedBy)
    {
        return new EmployeeShift
        {
            EmpShiftId = shiftId,
            EmpSysId = EmployeeId.Of(empSysId),
            TimeUnitId = timeUnitId,
            ShiftCode = code,
            YearMonth = yearMonth,
            Day = day,
            ShiftDate = shiftDate,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
    }
}
