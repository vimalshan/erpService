namespace AuthorizationService.Domain.Events;

/// <summary>
/// Event fired when a user right is granted
/// </summary>
public class UserRightGrantedEvent : DomainEvent
{
    public string? UserId { get; }
    public decimal? RightCode { get; }

    public UserRightGrantedEvent(long aggregateId, string? userId, decimal? rightCode)
        : base(aggregateId, DateTime.UtcNow)
    {
        UserId = userId;
        RightCode = rightCode;
    }
}
