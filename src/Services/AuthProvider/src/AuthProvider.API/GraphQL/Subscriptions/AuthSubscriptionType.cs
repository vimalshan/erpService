using AuthProvider.Application.DTOs;
using HotChocolate.Authorization;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace AuthProvider.API.GraphQL.Subscriptions;

/// <summary>
/// GraphQL Subscription type – real-time push events over WebSocket.
/// Clients can subscribe to user registration events.
/// </summary>
public sealed class AuthSubscriptionType
{
    /// <summary>Subscribe to new user registrations.</summary>
    [Authorize(Roles = new[] { "ADMIN" })]
    [Subscribe(With = nameof(SubscribeToUserRegistered))]
    public UserDto OnUserRegistered([EventMessage] UserDto user) => user;

    public ValueTask<ISourceStream<UserDto>> SubscribeToUserRegistered(
        [Service] ITopicEventReceiver receiver,
        CancellationToken ct)
        => receiver.SubscribeAsync<UserDto>("UserRegistered", ct);

    /// <summary>Subscribe to token events (revocations, expirations).</summary>
    [Authorize]
    [Subscribe(With = nameof(SubscribeToTokenEvents))]
    public string OnTokenEvent([EventMessage] string message) => message;

    public ValueTask<ISourceStream<string>> SubscribeToTokenEvents(
        [Service] ITopicEventReceiver receiver,
        CancellationToken ct)
        => receiver.SubscribeAsync<string>("TokenEvent", ct);
}
