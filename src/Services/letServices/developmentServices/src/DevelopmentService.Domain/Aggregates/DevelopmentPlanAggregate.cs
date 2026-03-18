using DevelopmentService.Domain.Entities;
using DevelopmentService.Domain.Events;

namespace DevelopmentService.Domain.Aggregates;

public class DevelopmentPlanAggregate
{
    private readonly List<IDomainEvent> _domainEvents = new();
    private readonly List<LetPlanProb> _probableItems = new();
    private readonly List<ReqNumCompeInd> _competencyLinks = new();

    public LetPlan Plan { get; private set; } = null!;
    public LetBhrPlan? BhrPlan { get; private set; }

    public IReadOnlyList<LetPlanProb> ProbableItems => _probableItems.AsReadOnly();
    public IReadOnlyList<ReqNumCompeInd> CompetencyLinks => _competencyLinks.AsReadOnly();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private DevelopmentPlanAggregate() { }

    public static DevelopmentPlanAggregate CreateNew(long reqNum, string userId, long pinNum,
        string devSource, string devNeed, long priority, DateTime entDate)
    {
        var aggregate = new DevelopmentPlanAggregate();
        aggregate.Plan = LetPlan.Create(reqNum, userId, pinNum, devSource, devNeed, priority, entDate);
        aggregate._domainEvents.AddRange(aggregate.Plan.DomainEvents);
        aggregate.Plan.ClearDomainEvents();
        return aggregate;
    }

    public static DevelopmentPlanAggregate Load(LetPlan plan) =>
        new() { Plan = plan };

    public void AttachBhrPlan(LetBhrPlan bhrPlan)
    {
        BhrPlan = bhrPlan;
        _domainEvents.AddRange(bhrPlan.DomainEvents);
        bhrPlan.ClearDomainEvents();
    }

    public void AddCompetencyLink(ReqNumCompeInd link) => _competencyLinks.Add(link);
    public void AddProbableItem(LetPlanProb item) => _probableItems.Add(item);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
