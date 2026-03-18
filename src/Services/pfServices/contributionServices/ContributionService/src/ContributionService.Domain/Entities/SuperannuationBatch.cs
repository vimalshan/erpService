using ContributionService.Domain.Events;

namespace ContributionService.Domain.Entities;

public class SuperannuationBatch : BaseEntity
{
    public long SnBatchNo { get; private set; }
    public long? SnTrustCode { get; private set; }
    public string? SnCategory { get; private set; }
    public string? SnPayunitCode { get; private set; }
    public string? SnPayMonthStart { get; private set; }
    public DateTime? SnPayMonthEnd { get; private set; }
    public string? SnStatus { get; private set; }
    public string? SnEntOn { get; private set; }
    public string? SnConAmt { get; private set; }
    public DateTime? SnPayDate { get; private set; }

    private SuperannuationBatch() { }

    public static SuperannuationBatch Create(
        long batchNo, long? trustCode, string? category, string? payunitCode,
        string? payMonthStart, DateTime? payMonthEnd, string? conAmt, DateTime? payDate)
    {
        var entity = new SuperannuationBatch
        {
            SnBatchNo = batchNo,
            SnTrustCode = trustCode,
            SnCategory = category,
            SnPayunitCode = payunitCode,
            SnPayMonthStart = payMonthStart,
            SnPayMonthEnd = payMonthEnd,
            SnStatus = "P",
            SnEntOn = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            SnConAmt = conAmt,
            SnPayDate = payDate
        };

        entity.AddDomainEvent(new SuperannuationBatchCreatedEvent(batchNo));
        return entity;
    }

    public void Approve()
    {
        SnStatus = "A";
    }
}
