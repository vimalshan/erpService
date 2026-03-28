namespace TransactionService.Domain.Entities;

public class SaaSubmit : BaseEntity
{
    public long PeriodId { get; set; }
    public long BusId { get; set; }
    public char BhrFlag { get; set; }
    public char ChrFlag { get; set; }
    public long BhrUpdBy { get; set; }
    public DateTime BhrUpdOn { get; set; }
    public decimal? BhrAmount { get; set; }
    public long? ChrUpdBy { get; set; }
    public DateTime? ChrUpdOn { get; set; }
    public decimal? ChrAmount { get; set; }

    public SaaPeriod? Period { get; set; }

    public SaaSubmit() { }

    public SaaSubmit(long periodId, long busId, long bhrUpdBy)
    {
        PeriodId = periodId;
        BusId = busId;
        BhrFlag = 'N';
        ChrFlag = 'N';
        BhrUpdBy = bhrUpdBy;
        BhrUpdOn = DateTime.UtcNow;
    }
}
