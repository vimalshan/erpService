using MediatR;
using UserSecurityService.Application.DTOs;
using UserSecurityService.Application.Features.UserProfile.Queries;

namespace UserSecurityService.API.MinimalApis;

public static class UserProfileEndpoints
{
    public static IEndpointRouteBuilder MapUserProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/users")
            .WithTags("Users (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllActiveUsersQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllUsersMinimal")
        .Produces<IEnumerable<UserProfileDto>>();

        group.MapGet("/{userId}", async (string userId, IMediator mediator, CancellationToken ct) =>
        {
            var profile = await mediator.Send(new GetUserProfileByIdQuery(userId), ct);
            return profile is null ? Results.NotFound() : Results.Ok(profile);
        })
        .WithName("GetUserByIdMinimal")
        .Produces<UserProfileDto>()
        .Produces(404);

        return app;
    }
}
