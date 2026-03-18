using DispatchPlanning.Domain.Common;
using DispatchPlanning.Domain.Entities;
using DispatchPlanning.Domain.Events;
using DispatchPlanning.Domain.Exceptions;
using DispatchPlanning.Domain.ValueObjects;

namespace DispatchPlanning.Domain.Aggregates;

/// <summary>
/// Aggregate Root: Dispatch Plan Header owns all plan items and sub-group targets.
/// </summary>
public class DispatchPlanAggregate : Entity
{
    public int DispatchPlanHeaderId { get; private set; }
    public PlanType PlanType { get; private set; } = default!;
    public DateTime PlanMonth { get; private set; }
    public string? PlanMPlus1 { get; private set; }
    public string? PlanMPlus2 { get; private set; }
    public string? PlanMPlus3 { get; private set; }
    public string? PlanMPlus4 { get; private set; }
    public DateTime EntryDate { get; private set; }
    public int CompanyUnitId { get; private set; }
    public int SciUserIdModified { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private readonly List<DispatchPlanItemwise> _items = new();
    private readonly List<DispatchPlanSubGroupwise> _subGroupTargets = new();

    public IReadOnlyCollection<DispatchPlanItemwise> Items => _items.AsReadOnly();
    public IReadOnlyCollection<DispatchPlanSubGroupwise> SubGroupTargets => _subGroupTargets.AsReadOnly();

    private DispatchPlanAggregate() { }

    public static DispatchPlanAggregate Create(int id, char planType, DateTime planMonth,
        int companyUnitId, int modifiedBy,
        string? mPlus1 = null, string? mPlus2 = null, string? mPlus3 = null, string? mPlus4 = null)
    {
        var aggregate = new DispatchPlanAggregate
        {
            DispatchPlanHeaderId = id,
            PlanType = PlanType.From(planType),
            PlanMonth = planMonth,
            PlanMPlus1 = mPlus1,
            PlanMPlus2 = mPlus2,
            PlanMPlus3 = mPlus3,
            PlanMPlus4 = mPlus4,
            EntryDate = DateTime.UtcNow,
            CompanyUnitId = companyUnitId,
            SciUserIdModified = modifiedBy,
            ModifiedDate = DateTime.UtcNow
        };

        aggregate.RaiseDomainEvent(new DispatchPlanCreatedEvent(id, planType, planMonth, companyUnitId));
        return aggregate;
    }

    public void AddItemTarget(int breakupItemId, TargetWeeks targets, int modifiedBy)
    {
        if (_items.Any(i => i.BreakupItemId == breakupItemId))
            throw new DuplicateDispatchPlanItemException(breakupItemId, DispatchPlanHeaderId);

        var item = DispatchPlanItemwise.Create(DispatchPlanHeaderId, breakupItemId, targets, modifiedBy);
        _items.Add(item);
        RaiseDomainEvent(new DispatchPlanItemAddedEvent(DispatchPlanHeaderId, breakupItemId));
    }

    public void UpdateItemTarget(int breakupItemId, TargetWeeks targets, int modifiedBy)
    {
        var item = _items.FirstOrDefault(i => i.BreakupItemId == breakupItemId)
            ?? throw new DispatchPlanItemNotFoundException(breakupItemId);
        item.UpdateTargets(targets, modifiedBy);
    }

    public void AddSubGroupTarget(int subGroupId, TargetWeeks targets, int modifiedBy)
    {
        if (_subGroupTargets.Any(s => s.SubGroupId == subGroupId))
            throw new DuplicateDispatchPlanItemException(subGroupId, DispatchPlanHeaderId);

        var sg = DispatchPlanSubGroupwise.Create(DispatchPlanHeaderId, subGroupId, targets, modifiedBy);
        _subGroupTargets.Add(sg);
    }

    public void UpdatePlanForecasts(string? mPlus1, string? mPlus2, string? mPlus3, string? mPlus4, int modifiedBy)
    {
        PlanMPlus1 = mPlus1;
        PlanMPlus2 = mPlus2;
        PlanMPlus3 = mPlus3;
        PlanMPlus4 = mPlus4;
        SciUserIdModified = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
        RaiseDomainEvent(new DispatchPlanForecastUpdatedEvent(DispatchPlanHeaderId));
    }
}
