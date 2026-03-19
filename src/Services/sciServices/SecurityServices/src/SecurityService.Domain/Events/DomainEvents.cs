namespace SecurityService.Domain.Events;

public sealed record UserCreatedEvent(
    long UserId,
    string UserCode,
    string? UserName,
    string? Email,
    DateTime CreatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record RoleAssignedEvent(
    long UserId,
    long RoleId,
    string? RoleName,
    DateTime AssignedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record RoleRevokedEvent(
    long UserId,
    long RoleId,
    DateTime RevokedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record RoleCreatedEvent(
    long RoleId,
    string RoleName,
    DateTime CreatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record MenuCreatedEvent(
    long MenuId,
    string MenuName,
    DateTime CreatedAt) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
