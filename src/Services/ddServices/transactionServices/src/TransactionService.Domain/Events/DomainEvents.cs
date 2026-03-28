using MediatR;

namespace TransactionService.Domain.Events;

public abstract class DomainEvent : INotification
{
    public Guid EventId { get; } = Guid.NewGuid();
    public long AggregateId { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public int Version { get; set; } = 1;
}

public interface IDomainEventPublisher
{
    Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    Task PublishAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}

public class DemandCreatedEvent : DomainEvent
{
    public string DemandType { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string Priority { get; set; } = string.Empty;
}

public class DemandApprovedEvent : DomainEvent
{
    public long ApprovedBy { get; set; }
    public string? Remarks { get; set; }
}

public class DemandRejectedEvent : DomainEvent
{
    public long RejectedBy { get; set; }
    public string? Remarks { get; set; }
}

public class DemandCompletedEvent : DomainEvent
{
    public long CompletedBy { get; set; }
    public string? Remarks { get; set; }
}

public class RecommendationCreatedEvent : DomainEvent
{
    public long EmpSysId { get; set; }
    public long PeriodId { get; set; }
    public decimal? RecommendAmount { get; set; }
}

public class RecommendationApprovedEvent : DomainEvent
{
    public string ApproverRole { get; set; } = string.Empty;
    public long ApprovedBy { get; set; }
}

public class BudgetUpdatedEvent : DomainEvent
{
    public long BusinessId { get; set; }
    public decimal NewAmount { get; set; }
}
