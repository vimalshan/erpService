using AimsTransactionService.Domain.Common;

namespace AimsTransactionService.Domain.Entities;

public class AttendanceOvertime : Entity
{
    public long EmployeeSysId { get; private set; }
    public DateTime OvertimeDate { get; private set; }
    public decimal OvertimeHours { get; private set; }
    public char Status { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private AttendanceOvertime() { }

    public static AttendanceOvertime Create(
        long id,
        long employeeSysId,
        DateTime overtimeDate,
        decimal overtimeHours,
        long createdBy)
    {
        return new AttendanceOvertime
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            OvertimeDate = overtimeDate,
            OvertimeHours = overtimeHours,
            Status = 'N',
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }
}
