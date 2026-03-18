using AuthProvider.Application.Commands;
using AuthProvider.Application.DTOs;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using MediatR;

namespace AuthProvider.API.GraphQL.Mutations;

/// <summary>
/// GraphQL Mutation type – write-side operations (create, update, delete, auth).
/// </summary>
public sealed class AuthMutationType
{
    /// <summary>Register a new user.</summary>
    public async Task<UserDto> RegisterUser(
        [Service] IMediator mediator,
        [Service] ITopicEventSender eventSender,
        CreateUserInput input,
        CancellationToken ct)
    {
        var user = await mediator.Send(
            new CreateUserCommand(input.Username, input.Email, input.Password, input.FirstName, input.LastName), ct);

        // Publish subscription event
        await eventSender.SendAsync("UserRegistered", user, ct);
        return user;
    }

    /// <summary>Login – returns JWT tokens.</summary>
    public async Task<TokenResponseDto> Login(
        [Service] IMediator mediator,
        LoginInput input,
        [Service] IHttpContextAccessor httpContextAccessor,
        CancellationToken ct)
    {
        var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "unknown";
        return await mediator.Send(new LoginCommand(input.UsernameOrEmail, input.Password, ip), ct);
    }

    /// <summary>Update user profile.</summary>
    [Authorize]
    public async Task<UserDto> UpdateUser(
        [Service] IMediator mediator,
        UpdateUserInput input,
        CancellationToken ct)
        => await mediator.Send(new UpdateUserCommand(input.UserId, input.FirstName, input.LastName), ct);

    /// <summary>Assign a role to a user.</summary>
    [Authorize(Roles = new[] { "ADMIN" })]
    public async Task<bool> AssignRole(
        [Service] IMediator mediator,
        AssignRoleInput input,
        CancellationToken ct)
        => await mediator.Send(new AssignRoleCommand(input.UserId, input.RoleName), ct);

    /// <summary>Deactivate a user.</summary>
    [Authorize(Roles = new[] { "ADMIN" })]
    public async Task<bool> DeleteUser(
        [Service] IMediator mediator,
        Guid userId,
        CancellationToken ct)
        => await mediator.Send(new DeleteUserCommand(userId), ct);
}

// ─── GraphQL Input types ──────────────────────────────────────────────────────

public record CreateUserInput(string Username, string Email, string Password, string FirstName, string LastName);
public record LoginInput(string UsernameOrEmail, string Password);
public record UpdateUserInput(Guid UserId, string FirstName, string LastName);
public record AssignRoleInput(Guid UserId, string RoleName);
