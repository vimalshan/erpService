using UtilityService.Domain.Common;
using UtilityService.Domain.Entities;

namespace UtilityService.Domain.Aggregates;

/// <summary>
/// Aggregate root that manages a collection of ToadPlanSql entries for a user session.
/// </summary>
public class ToadPlanAggregate : BaseEntity
{
    private readonly List<ToadPlanSql> _plans = new();

    public string SessionOwner { get; private set; } = string.Empty;
    public DateTime SessionStarted { get; private set; }
    public IReadOnlyCollection<ToadPlanSql> Plans => _plans.AsReadOnly();

    private ToadPlanAggregate() { }

    public static ToadPlanAggregate CreateSession(string sessionOwner)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionOwner);
        return new ToadPlanAggregate
        {
            SessionOwner = sessionOwner,
            SessionStarted = DateTime.UtcNow
        };
    }

    public void AddPlan(ToadPlanSql plan)
    {
        ArgumentNullException.ThrowIfNull(plan);
        _plans.Add(plan);
    }

    public void RemovePlan(int planId)
    {
        var plan = _plans.FirstOrDefault(p => p.Id == planId)
            ?? throw new InvalidOperationException($"Plan {planId} not found in this session.");
        plan.Delete();
        _plans.Remove(plan);
    }

    public int TotalPlans => _plans.Count;
}
