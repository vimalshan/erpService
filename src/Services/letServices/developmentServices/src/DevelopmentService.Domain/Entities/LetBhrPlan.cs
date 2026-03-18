using DevelopmentService.Domain.Events;

namespace DevelopmentService.Domain.Entities;

public class LetBhrPlan
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public long ReqNum { get; private set; }
    public long? Sno { get; private set; }
    public string? UserId { get; private set; }
    public string? TrainingProgram { get; private set; }
    public decimal? TrainingCode { get; private set; }
    public decimal? Priority { get; private set; }
    public long? PiNum { get; private set; }
    public string? FinalAccept { get; private set; }
    public char? BhrAccept { get; private set; }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private LetBhrPlan() { }

    public static LetBhrPlan Create(long reqNum, string userId, string trainingProgram,
        decimal trainingCode, decimal priority, char bhrAccept)
    {
        var plan = new LetBhrPlan
        {
            ReqNum = reqNum,
            UserId = userId,
            TrainingProgram = trainingProgram,
            TrainingCode = trainingCode,
            Priority = priority,
            BhrAccept = bhrAccept
        };

        plan._domainEvents.Add(new BhrPlanCreatedEvent(reqNum, userId, trainingProgram));
        return plan;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
