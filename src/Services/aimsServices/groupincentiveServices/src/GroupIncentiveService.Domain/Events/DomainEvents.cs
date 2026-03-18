namespace GroupIncentiveService.Domain.Events;

public sealed record GroupIncentiveCreatedEvent(
    long IncentiveId,
    int GroupId,
    int Month,
    int Year,
    decimal TotalAmount,
    long CreatedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record GroupIncentiveApprovedEvent(
    long IncentiveId,
    int GroupId,
    decimal ApprovedAmount,
    long ApprovedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record GroupIncentiveRejectedEvent(
    long IncentiveId,
    int GroupId,
    string Remarks,
    long RejectedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record GroupCreatedEvent(
    int GroupId,
    string GroupName,
    long CreatedBy) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record EmployeeAddedToGroupEvent(
    long MappingId,
    int GroupId,
    long EmployeeId,
    string? Role) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
