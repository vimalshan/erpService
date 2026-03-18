using AuthProvider.Domain.Common;

namespace AuthProvider.Domain.Events;

public sealed class UserCreatedEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string Username { get; }

    public UserCreatedEvent(Guid userId, string email, string username)
    {
        UserId = userId;
        Email = email;
        Username = username;
    }
}
