using AimsTransactionService.Domain.Common;

namespace AimsTransactionService.Domain.Entities;

public class AttendanceLopMain : Entity
{
    public long EmployeeSysId { get; private set; }
    public DateTime MonthStart { get; private set; }
    public DateTime MonthEnd { get; private set; }
    public int CalendarDays { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private readonly List<AttendanceLopDetail> _details = [];
    public IReadOnlyCollection<AttendanceLopDetail> Details => _details.AsReadOnly();

    private AttendanceLopMain() { }

    public static AttendanceLopMain Create(
        long id,
        long employeeSysId,
        DateTime monthStart,
        DateTime monthEnd,
        int calendarDays,
        long createdBy)
    {
        return new AttendanceLopMain
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            MonthStart = monthStart,
            MonthEnd = monthEnd,
            CalendarDays = calendarDays,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void AddDetail(AttendanceLopDetail detail) => _details.Add(detail);
}
