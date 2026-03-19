using MediatR;
using MobileAppManagement.Application.Commands;
using MobileAppManagement.Application.Queries;

namespace MobileAppManagement.API.MinimalApis;

public static class LoginEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/logins")
            .WithTags("Logins (Minimal)")
            .RequireAuthorization();

        group.MapGet("/user/{userSysId}", async (decimal userSysId,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoginsByUserQuery(userSysId), ct);
            return Results.Ok(result);
        });

        group.MapGet("/{loginId}", async (decimal loginId,
            IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoginByIdQuery(loginId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (LogUserLoginCommand command,
            IMediator mediator, CancellationToken ct) =>
        {
            var loginId = await mediator.Send(command, ct);
            return Results.Ok(new { loginId });
        }).AllowAnonymous();
    }
}
