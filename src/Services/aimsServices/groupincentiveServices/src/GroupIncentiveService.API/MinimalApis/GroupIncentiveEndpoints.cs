using GroupIncentiveService.Application.Commands.CreateGroupMaster;
using GroupIncentiveService.Application.DTOs;
using GroupIncentiveService.Application.Queries.GetAllGroups;
using GroupIncentiveService.Application.Queries.GetEmployeeIncentive;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace GroupIncentiveService.API.MinimalApis;

public static class GroupIncentiveEndpoints
{
    public static IEndpointRouteBuilder MapGroupIncentiveMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/groups")
            .WithTags("Groups v2 (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, bool activeOnly, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllGroupsQuery(activeOnly), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllGroupsV2")
        .Produces<IEnumerable<GroupMasterDto>>();

        group.MapPost("/", async (CreateGroupMasterCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/groups/{id}", new { GroupId = id });
        })
        .WithName("CreateGroupV2")
        .Produces<object>(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        var empGroup = app.MapGroup("/api/v2/employees")
            .WithTags("Employee Incentives (Minimal API)")
            .RequireAuthorization();

        empGroup.MapGet("/{employeeId:long}/incentive", async (
            long employeeId, int month, int year, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetEmployeeIncentiveQuery(employeeId, month, year), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetEmployeeIncentive")
        .Produces<EmployeeIncentiveSummaryDto>()
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
