using MasterService.Application.Features.Skills.Commands;
using MasterService.Application.Features.Skills.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace MasterService.API.MinimalApis;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/skills")
            .WithTags("Skills (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, char? skillType, CancellationToken ct)
            => Results.Ok(await mediator.Send(new GetSkillsQuery(skillType), ct)));

        group.MapGet("/{skillCode:long}", async (IMediator mediator, long skillCode, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSkillByCodeQuery(skillCode), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (IMediator mediator, CreateSkillCommand command, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/skills/{result.SkillCode}", result);
        });

        group.MapDelete("/{skillCode:long}", async (IMediator mediator, long skillCode, CancellationToken ct) =>
        {
            await mediator.Send(new CloseSkillCommand(skillCode), ct);
            return Results.NoContent();
        });
    }
}
