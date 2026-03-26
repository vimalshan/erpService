using AimsTransactionService.Domain.Common;

namespace AimsTransactionService.Domain.Entities;

public class AttendanceSummary : Entity
{
    public long EmployeeSysId { get; private set; }
    public DateTime MonthStart { get; private set; }
    public DateTime MonthEnd { get; private set; }
    public int WorkingDays { get; private set; }
    public int PresentDays { get; private set; }
    public int AbsentDays { get; private set; }
    public decimal OvertimeHours { get; private set; }
    public int LopDays { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private AttendanceSummary() { }

    public static AttendanceSummary Create(
        long id,
        long employeeSysId,
        DateTime monthStart,
        DateTime monthEnd,
        int workingDays,
        int presentDays,
        int absentDays,
        decimal overtimeHours,
        int lopDays,
        long createdBy)
    {
        return new AttendanceSummary
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            MonthStart = monthStart,
            MonthEnd = monthEnd,
            WorkingDays = workingDays,
            PresentDays = presentDays,
            AbsentDays = absentDays,
            OvertimeHours = overtimeHours,
            LopDays = lopDays,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }
}
