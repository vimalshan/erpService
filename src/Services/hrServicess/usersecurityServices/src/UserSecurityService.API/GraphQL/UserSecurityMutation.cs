using HotChocolate;
using MediatR;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Features.UserProfile.Commands;

namespace UserSecurityService.API.GraphQL;

public class UserSecurityMutation
{
    [GraphQLDescription("Create a new user profile.")]
    public async Task<UserProfileDto> CreateUser(
        [Service] IMediator mediator,
        string userId, decimal empNum, string unitCode, string nickName,
        string userType, string emailFlag, DateTime effectiveDate,
        string password, string regStatus, string? empName,
        CancellationToken ct)
        => await mediator.Send(new CreateUserProfileCommand(
            userId, empNum, unitCode, nickName, userType, emailFlag,
            effectiveDate, password, regStatus, empName), ct);

    [GraphQLDescription("Deactivate a user profile.")]
    public async Task<bool> DeactivateUser(
        [Service] IMediator mediator, string userId, CancellationToken ct)
    {
        await mediator.Send(new DeactivateUserCommand(userId), ct);
        return true;
    }
}
