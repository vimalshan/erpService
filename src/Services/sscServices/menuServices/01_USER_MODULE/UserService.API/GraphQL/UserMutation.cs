using MediatR;
using UserService.Application.Commands;
using UserService.Application.DTOs;

namespace UserService.API.GraphQL;

/// <summary>
/// GraphQL Mutation root — all write operations.
/// </summary>
public class UserMutation
{
    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    public async Task<LoginResponse> LoginAsync(
        string email,
        string password,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new LoginUserCommand { Email = email, Password = password },
            cancellationToken);

    /// <summary>
    /// Creates a new user. Returns the new user's numeric ID.
    /// </summary>
    public async Task<long> CreateUserAsync(
        CreateUserInput input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new CreateUserCommand
            {
                UserName    = input.UserName,
                Password    = input.Password,
                EmailId     = input.EmailId,
                EnteredBy   = input.EnteredBy,
                SparchUserId = input.SparchUserId,
                HrEmpSysId  = input.HrEmpSysId
            },
            cancellationToken);

    /// <summary>
    /// Updates an existing user's profile details.
    /// </summary>
    public async Task<bool> UpdateUserAsync(
        UpdateUserInput input,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new UpdateUserCommand
            {
                UserId       = input.UserId,
                UserName     = input.UserName,
                EmailId      = input.EmailId,
                SparchUserId = input.SparchUserId
            },
            cancellationToken);

    /// <summary>
    /// Deactivates a user (soft-delete).
    /// </summary>
    public async Task<bool> DeactivateUserAsync(
        long userId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(new DeactivateUserCommand { UserId = userId }, cancellationToken);

    /// <summary>
    /// Assigns a role to a user.
    /// </summary>
    public async Task<bool> AssignRoleAsync(
        long userId,
        long roleId,
        bool isDefault,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new AssignRoleToUserCommand { UserId = userId, RoleId = roleId, IsDefault = isDefault },
            cancellationToken);

    /// <summary>
    /// Assigns a user to a business unit (organisation).
    /// </summary>
    public async Task<bool> AssignOrganizationAsync(
        long userId,
        string businessUnitId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new AssignOrganizationToUserCommand { UserId = userId, BusinessUnitId = businessUnitId },
            cancellationToken);

    /// <summary>
    /// Assigns a user to a location.
    /// </summary>
    public async Task<bool> AssignLocationAsync(
        long userId,
        int locationId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new AssignLocationToUserCommand { UserId = userId, LocationId = locationId },
            cancellationToken);
}

// ── Input types ──────────────────────────────────────────────────────────────

public record CreateUserInput(
    string UserName,
    string Password,
    string EmailId,
    long   EnteredBy,
    string? SparchUserId = null,
    long?   HrEmpSysId  = null);

public record UpdateUserInput(
    long   UserId,
    string UserName,
    string EmailId,
    string? SparchUserId = null);
