using MediatR;
using GroupManagementService.Application.Commands;
using GroupManagementService.Application.Queries;
using GroupManagementService.Application.DTOs;

namespace GroupManagementService.API.Endpoints;

/// <summary>
/// Minimal API endpoints for Group Management (alternative to controllers)
/// </summary>
public static class GroupEndpoints
{
    public static void MapGroupEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/groups")
            .WithTags("Minimal-Groups");

        group.MapGet("/", GetAllGroups)
            .WithName("MinimalGetAllGroups")
            .AllowAnonymous();

        group.MapGet("/{id:long}", GetGroupById)
            .WithName("MinimalGetGroupById")
            .AllowAnonymous();

        group.MapGet("/code/{code}", GetGroupByCode)
            .WithName("MinimalGetGroupByCode")
            .AllowAnonymous();

        group.MapPost("/", CreateGroup)
            .WithName("MinimalCreateGroup")
            .AllowAnonymous();

        group.MapPut("/{id:long}", UpdateGroup)
            .WithName("MinimalUpdateGroup")
            .AllowAnonymous();

        group.MapPost("/{id:long}/activate", ActivateGroup)
            .WithName("MinimalActivateGroup")
            .AllowAnonymous();

        group.MapPost("/{id:long}/deactivate", DeactivateGroup)
            .WithName("MinimalDeactivateGroup")
            .AllowAnonymous();
    }

    private static async Task<IResult> GetAllGroups(IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllGroupsQuery(), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetGroupById(long id, IMediator mediator, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new GetGroupByIdQuery(id), ct);
            return Results.Ok(result);
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound(new { message = $"Group {id} not found" });
        }
    }

    private static async Task<IResult> GetGroupByCode(string code, IMediator mediator, CancellationToken ct)
    {
        try
        {
            var result = await mediator.Send(new GetGroupByCodeQuery(code), ct);
            return Results.Ok(result);
        }
        catch (InvalidOperationException)
        {
            return Results.NotFound(new { message = $"Group '{code}' not found" });
        }
    }

    private static async Task<IResult> CreateGroup(
        CreateGroupRequest request, IMediator mediator, CancellationToken ct)
    {
        try
        {
            var command = new CreateGroupCommand(request.Code, request.Name, request.Description, request.CreatedBy, request.IsAdmin);
            var result = await mediator.Send(command, ct);
            return Results.CreatedAtRoute("MinimalGetGroupById", new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.Conflict(new { message = ex.Message });
        }
    }

    private static async Task<IResult> UpdateGroup(
        long id, UpdateGroupRequest request, IMediator mediator, CancellationToken ct)
    {
        try
        {
            var command = new UpdateGroupCommand(id, request.Name, request.Description, request.UpdatedBy);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
    }

    private static async Task<IResult> ActivateGroup(long id, IMediator mediator, CancellationToken ct)
    {
        try
        {
            await mediator.Send(new ActivateGroupCommand(id, 1), ct);
            return Results.Ok(new { message = "Group activated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
    }

    private static async Task<IResult> DeactivateGroup(long id, IMediator mediator, CancellationToken ct)
    {
        try
        {
            await mediator.Send(new DeactivateGroupCommand(id, 1), ct);
            return Results.Ok(new { message = "Group deactivated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
    }
}
