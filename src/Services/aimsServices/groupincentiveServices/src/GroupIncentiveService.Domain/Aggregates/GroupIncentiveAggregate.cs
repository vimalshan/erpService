using GroupIncentiveService.Domain.Entities;
using GroupIncentiveService.Domain.Events;
using GroupIncentiveService.Domain.Exceptions;

namespace GroupIncentiveService.Domain.Aggregates;

/// <summary>
/// Aggregate root for the Group Incentive bounded context.
/// Encapsulates GroupIncentiveMain along with its details and approval history.
/// </summary>
public sealed class GroupIncentiveAggregate : BaseEntity
{
    public GroupIncentiveMain Incentive { get; private set; }
    public IReadOnlyCollection<GroupIncentiveDet> Details => Incentive.Details;
    public IReadOnlyCollection<GroupIncentiveApproval> Approvals => Incentive.Approvals;

    private GroupIncentiveAggregate(GroupIncentiveMain incentive)
    {
        Incentive = incentive;
    }

    public static GroupIncentiveAggregate Create(long id, int groupId, int month, int year,
        decimal totalAmount, long createdBy)
    {
        var incentive = GroupIncentiveMain.Create(id, groupId, month, year, totalAmount, createdBy);
        return new GroupIncentiveAggregate(incentive);
    }

    public void AddDetail(long detId, long employeeId, decimal allocPercentage, decimal allocAmount, long createdBy)
    {
        var detail = GroupIncentiveDet.Create(detId, Incentive.GrpIncId, employeeId,
            allocPercentage, allocAmount, createdBy);
        // Details added through EF navigation
    }

    public void Approve(decimal approvedAmount, long approvedBy, long approvalRecordId)
    {
        Incentive.Approve(approvedAmount, approvedBy);
        var approval = GroupIncentiveApproval.Create(
            approvalRecordId, Incentive.GrpIncId, approvedBy, "Y", null, approvedBy);

        foreach (var evt in Incentive.DomainEvents)
            AddDomainEvent(evt);
    }

    public void Reject(long rejectedBy, string remarks, long approvalRecordId)
    {
        Incentive.Reject(rejectedBy, remarks);
        var approval = GroupIncentiveApproval.Create(
            approvalRecordId, Incentive.GrpIncId, rejectedBy, "N", remarks, rejectedBy);

        foreach (var evt in Incentive.DomainEvents)
            AddDomainEvent(evt);
    }

    public decimal TotalAllocatedPercentage => Details.Sum(d => d.GrpIncDetAllocPercentage);

    public void ValidateAllocations()
    {
        var total = TotalAllocatedPercentage;
        if (total > 100)
            throw new DomainException($"Total allocation percentage {total} exceeds 100%.");
    }
}
