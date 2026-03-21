using ComplaintService.Domain.Common;

namespace ComplaintService.Domain.Entities;

/// <summary>Maps to COMPL_TASK — complaint task scheduling.</summary>
public class ComplaintTask : BaseEntity
{
    public decimal TaskNum { get; private set; }            // CT_TASK_NUM (PK)
    public decimal TicketNum { get; private set; }          // CT_TICKET_NUM
    public string ScheduleFreq { get; private set; } = default!; // CT_SCHEDULE_FREQ
    public string? ScheduleValue { get; private set; }      // CT_SCHEDULE_VALUE
    public string? ScheduleTime { get; private set; }       // CT_SCHEDULE_TIME
    public string? ScheduleDay { get; private set; }        // CT_SCHEDULE_DAY
    public DateTime? EffDate { get; private set; }          // CT_EFF_DATE
    public DateTime? ClsDate { get; private set; }          // CT_CLS_DATE
    public decimal? UpdatedBy { get; private set; }         // CT_UPDATED_BY
    public DateTime? UpdatedOn { get; private set; }        // CT_UPDATED_ON

    protected ComplaintTask() { }

    public static ComplaintTask Create(
        decimal taskNum, decimal ticketNum, string schedFreq,
        string? schedValue = null, string? schedTime = null, string? schedDay = null) =>
        new()
        {
            TaskNum = taskNum,
            TicketNum = ticketNum,
            ScheduleFreq = schedFreq,
            ScheduleValue = schedValue,
            ScheduleTime = schedTime,
            ScheduleDay = schedDay,
            EffDate = DateTime.UtcNow
        };

    public void Deactivate(decimal updatedBy)
    {
        ClsDate = DateTime.UtcNow;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
