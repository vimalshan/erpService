using ContributionService.Domain.Events;

namespace ContributionService.Domain.Entities;

public class ContributionMain : BaseEntity
{
    public long ContributionBatchNo { get; private set; }
    public string ContributionTrustCode { get; private set; } = null!;
    public string ContributionCategory { get; private set; } = null!;
    public string ContributionPayunitCode { get; private set; } = null!;
    public DateTime ContributionPayMonthStart { get; private set; }
    public DateTime ContributionPayMonthEnd { get; private set; }
    public string ContributionStatus { get; private set; } = null!;
    public decimal? ContributionJvNo { get; private set; }
    public decimal? ContributionRecActranNo { get; private set; }
    public DateTime? ContributionEntOn { get; private set; }
    public long ContributionRefNo { get; private set; }

    private readonly List<ContributionDetail> _details = [];
    public IReadOnlyCollection<ContributionDetail> Details => _details.AsReadOnly();

    private ContributionMain() { }

    public static ContributionMain Create(
        long batchNo, string trustCode, string category, string payunitCode,
        DateTime payMonthStart, DateTime payMonthEnd, long refNo)
    {
        var entity = new ContributionMain
        {
            ContributionBatchNo = batchNo,
            ContributionTrustCode = trustCode,
            ContributionCategory = category,
            ContributionPayunitCode = payunitCode,
            ContributionPayMonthStart = payMonthStart,
            ContributionPayMonthEnd = payMonthEnd,
            ContributionStatus = "P",
            ContributionRefNo = refNo,
            ContributionEntOn = DateTime.UtcNow
        };

        entity.AddDomainEvent(new ContributionBatchCreatedEvent(batchNo, trustCode, payunitCode));
        return entity;
    }

    public void Post(long postedByUserId)
    {
        if (ContributionStatus == "PO")
            throw new InvalidOperationException("Batch is already posted.");

        ContributionStatus = "PO";
        ContributionEntOn = DateTime.UtcNow;
        AddDomainEvent(new ContributionBatchPostedEvent(ContributionBatchNo, postedByUserId));
    }

    public void UpdateStatus(string status)
    {
        ContributionStatus = status;
        AddDomainEvent(new ContributionStatusChangedEvent(ContributionBatchNo, status));
    }

    public void AddDetail(ContributionDetail detail) => _details.Add(detail);
}
