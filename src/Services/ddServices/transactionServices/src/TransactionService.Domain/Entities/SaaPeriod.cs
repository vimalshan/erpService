namespace TransactionService.Domain.Entities;

public class SaaPeriod : BaseEntity
{
    public long YearId { get; set; }
    public long QuarterNo { get; set; }
    public char Status { get; set; } = 'O';
    public DateTime PeriodOpenDate { get; set; }
    public DateTime PeriodCloseDate { get; set; }
    public DateTime? CircularGenOn { get; set; }
    public long? CircularGenBy { get; set; }
    public DateTime? ReminderLetOn { get; set; }
    public DateTime FormOpenDate { get; set; }
    public DateTime? AppraiserLastDate { get; set; }
    public DateTime? ReviewerLastDate { get; set; }
    public DateTime? BhrLastDate { get; set; }
    public DateTime? UhrLastDate { get; set; }

    public SaaPeriod() { }

    public SaaPeriod(long yearId, long quarterNo, DateTime periodOpenDate, DateTime periodCloseDate, DateTime formOpenDate)
    {
        YearId = yearId;
        QuarterNo = quarterNo;
        Status = 'O';
        PeriodOpenDate = periodOpenDate;
        PeriodCloseDate = periodCloseDate;
        FormOpenDate = formOpenDate;
    }

    public bool IsOpen => Status == 'O';
}
