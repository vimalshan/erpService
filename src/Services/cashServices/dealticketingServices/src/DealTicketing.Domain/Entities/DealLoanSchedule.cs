using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Entities;

public class DealLoanSchedule : BaseEntity
{
    public long DealSchId { get; private set; }
    public long DealId { get; private set; }
    public DateTime DealSchDate { get; private set; }
    public long DealSchAmt { get; private set; }

    public DealDetail DealDetail { get; private set; } = default!;

    private DealLoanSchedule() { }

    public DealLoanSchedule(long schId, long dealId, DateTime schDate, long schAmt)
    {
        DealSchId = schId;
        DealId = dealId;
        DealSchDate = schDate;
        DealSchAmt = schAmt;
    }
}
