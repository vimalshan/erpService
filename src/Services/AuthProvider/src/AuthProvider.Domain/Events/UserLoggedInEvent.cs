using AuthProvider.Domain.Common;

namespace AuthProvider.Domain.Events;

public sealed class UserLoggedInEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }

    public UserLoggedInEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }
}
