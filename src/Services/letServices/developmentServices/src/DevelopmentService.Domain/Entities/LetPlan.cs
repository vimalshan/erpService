using DevelopmentService.Domain.Events;

namespace DevelopmentService.Domain.Entities;

public class LetPlan
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public long ReqNum { get; private set; }
    public long? Sno { get; private set; }
    public string? UserId { get; private set; }
    public long? PinNum { get; private set; }
    public string? DevSource { get; private set; }
    public string? DevNeed { get; private set; }
    public string? DevIndicator { get; private set; }
    public long? DevMode { get; private set; }
    public string? RecProg { get; private set; }
    public string? TrainingProgram { get; private set; }
    public long? InternalTraining { get; private set; }
    public string? RevDate { get; private set; }
    public long? Priority { get; private set; }
    public DateTime? EntDate { get; private set; }
    public char? AppStatus { get; private set; }
    public char? BhrStatus { get; private set; }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private LetPlan() { }

    public static LetPlan Create(long reqNum, string userId, long pinNum, string devSource,
        string devNeed, long priority, DateTime entDate)
    {
        var plan = new LetPlan
        {
            ReqNum = reqNum,
            UserId = userId,
            PinNum = pinNum,
            DevSource = devSource,
            DevNeed = devNeed,
            Priority = priority,
            EntDate = entDate,
            AppStatus = 'F'
        };

        plan._domainEvents.Add(new LearningPlanCreatedEvent(reqNum, userId, devNeed));
        return plan;
    }

    public void Approve(char appStatus, char? bhrStatus = null)
    {
        AppStatus = appStatus;
        if (bhrStatus.HasValue) BhrStatus = bhrStatus;
        _domainEvents.Add(new LearningPlanApprovedEvent(ReqNum, appStatus));
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
