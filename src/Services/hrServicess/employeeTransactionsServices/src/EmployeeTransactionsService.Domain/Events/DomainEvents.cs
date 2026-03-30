using EmployeeTransactionsService.Domain.Common;

namespace EmployeeTransactionsService.Domain.Events;

public sealed record EmployeeCreatedDomainEvent(decimal EmployeeId, string FullName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record EmployeeGradeChangedDomainEvent(decimal GradeChangeId, decimal EmployeeId, decimal OldGradeId, decimal NewGradeId) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record ProbationReviewedDomainEvent(decimal ProbationId, decimal EmployeeId, string FinalStatus) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record AlertGroupCreatedDomainEvent(decimal AlertGroupId, string GroupName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}

public sealed record StationeryImageUploadedDomainEvent(Guid ImageId, string ItemReference, string BlobName) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}