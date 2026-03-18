using HotChocolate;
using HotChocolate.Types;
using MediatR;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Features.UserProfile.Queries;

namespace UserSecurityService.API.GraphQL;

public class UserSecurityQuery
{
    [GraphQLDescription("Fetch a user profile by user ID.")]
    public async Task<UserProfileDto?> GetUserProfile(
        [Service] IMediator mediator, string userId, CancellationToken ct)
        => await mediator.Send(new GetUserProfileByIdQuery(userId), ct);

    [GraphQLDescription("Fetch all active user profiles.")]
    public async Task<IEnumerable<UserProfileDto>> GetAllUsers(
        [Service] IMediator mediator, CancellationToken ct)
        => await mediator.Send(new GetAllActiveUsersQuery(), ct);

    [GraphQLDescription("Fetch a user profile by employee number.")]
    public async Task<UserProfileDto?> GetUserByEmpNum(
        [Service] IMediator mediator, decimal empNum, CancellationToken ct)
        => await mediator.Send(new GetUserProfileByEmpNumQuery(empNum), ct);
}

public class UserProfileType : ObjectType<UserProfileDto>
{
    protected override void Configure(IObjectTypeDescriptor<UserProfileDto> descriptor)
    {
        descriptor.Description("Represents a user security profile.");
        descriptor.Field(f => f.UserId).Description("The unique user identifier.");
        descriptor.Field(f => f.EmpNum).Description("Employee number.");
        descriptor.Field(f => f.PhotoPath).Description("URL to the user photo stored in Azure Blob Storage.");
    }
}
