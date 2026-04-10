using TaskTransactional.Domain.Common;

namespace TaskTransactional.Domain.Entities;

public class ComplaintTask : BaseEntity
{
    public decimal CtTaskNum { get; private set; }
    public decimal CtTicketNum { get; private set; }
    public string CtScheduleFreq { get; private set; } = null!;
    public string? CtScheduleValue { get; private set; }
    public string? CtScheduleTime { get; private set; }
    public string? CtScheduleDay { get; private set; }
    public DateTime? CtEffDate { get; private set; }
    public DateTime? CtClsDate { get; private set; }
    public decimal? CtUpdatedBy { get; private set; }
    public DateTime? CtUpdatedOn { get; private set; }

    // Navigation
    public ComplaintDetail? Detail { get; private set; }

    private ComplaintTask() { }

    public static ComplaintTask Create(
        decimal taskNum, decimal ticketNum, string scheduleFreq,
        string? scheduleValue = null, string? scheduleTime = null, string? scheduleDay = null)
    {
        return new ComplaintTask
        {
            CtTaskNum = taskNum,
            CtTicketNum = ticketNum,
            CtScheduleFreq = scheduleFreq,
            CtScheduleValue = scheduleValue,
            CtScheduleTime = scheduleTime,
            CtScheduleDay = scheduleDay,
            CtEffDate = DateTime.UtcNow
        };
    }

    public void Close(decimal updatedBy)
    {
        CtClsDate = DateTime.UtcNow;
        CtUpdatedBy = updatedBy;
        CtUpdatedOn = DateTime.UtcNow;
    }
}
