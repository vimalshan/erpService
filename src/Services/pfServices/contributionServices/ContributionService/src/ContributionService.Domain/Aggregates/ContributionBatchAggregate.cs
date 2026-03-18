using ContributionService.Domain.Entities;

namespace ContributionService.Domain.Aggregates;

public class ContributionBatchAggregate
{
    public ContributionMain Batch { get; }
    public List<ContributionDetail> Details { get; } = [];
    public List<ContributionBreakup> Breakups { get; } = [];

    public ContributionBatchAggregate(ContributionMain batch)
    {
        Batch = batch;
    }

    public void AddDetail(ContributionDetail detail)
    {
        Details.Add(detail);
        Batch.AddDetail(detail);
    }

    public decimal TotalEeAmount => Details.Sum(d => d.ContributionEeAmount);
    public decimal TotalErAmount => Details.Sum(d => d.ContributionErAmount);
    public decimal TotalContribution => TotalEeAmount + TotalErAmount;
    public int MemberCount => Details.Select(d => d.ContributionMemberNo).Distinct().Count();

    public void ValidateAll()
    {
        foreach (var detail in Details)
            detail.Validate();
    }

    public void Post(long postedByUserId) => Batch.Post(postedByUserId);
}
