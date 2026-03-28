namespace AuthorizationService.Domain.Events;

/// <summary>
/// Event fired when a user right is revoked
/// </summary>
public class UserRightRevokedEvent : DomainEvent
{
    public string? UserId { get; }
    public decimal? RightCode { get; }

    public UserRightRevokedEvent(long aggregateId, string? userId, decimal? rightCode)
        : base(aggregateId, DateTime.UtcNow)
    {
        UserId = userId;
        RightCode = rightCode;
        Version = 2;
    }
}
