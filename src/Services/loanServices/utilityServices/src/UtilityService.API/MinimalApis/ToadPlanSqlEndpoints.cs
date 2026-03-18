using MediatR;
using UtilityService.Application.Commands.CreateToadPlanSql;
using UtilityService.Application.Commands.DeleteToadPlanSql;
using UtilityService.Application.DTOs;
using UtilityService.Application.Queries.GetAllToadPlanSql;
using UtilityService.Application.Queries.GetToadPlanSqlById;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UtilityService.API.MinimalApis;

public static class ToadPlanSqlEndpoints
{
    public static IEndpointRouteBuilder MapToadPlanSqlMinimalApis(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/minimal/toadplansql")
            .WithTags("ToadPlanSql-Minimal")
            .RequireAuthorization();

        group.MapGet("/", GetAll)
            .WithName("MinimalGetAllToadPlanSql")
            .WithSummary("Get all TOAD plan entries (minimal API).")
            .Produces<PagedResultDto<ToadPlanSqlDto>>();

        group.MapGet("/{id:int}", GetById)
            .WithName("MinimalGetToadPlanSqlById")
            .WithSummary("Get a TOAD plan entry by ID (minimal API).")
            .Produces<ToadPlanSqlDto>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateEntry)
            .WithName("MinimalCreateToadPlanSql")
            .WithSummary("Create a new TOAD plan entry (minimal API).")
            .Produces<ToadPlanSqlDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteEntry)
            .WithName("MinimalDeleteToadPlanSql")
            .WithSummary("Delete a TOAD plan entry (minimal API).")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization("AdminPolicy");

        return app;
    }

    private static async Task<IResult> GetAll(
        IMediator mediator,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllToadPlanSqlQuery(pageNumber, pageSize), ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetById(
        int id, IMediator mediator, CancellationToken ct)
    {
        var result = await mediator.Send(new GetToadPlanSqlByIdQuery(id), ct);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> CreateEntry(
        CreateToadPlanSqlDto dto, IMediator mediator, CancellationToken ct)
    {
        var command = new CreateToadPlanSqlCommand(dto.Username, dto.StatementId, dto.Statement, dto.Timestamp);
        var result = await mediator.Send(command, ct);
        return Results.Created($"/api/v1/minimal/toadplansql/{result.Id}", result);
    }

    private static async Task<IResult> DeleteEntry(
        int id, IMediator mediator, CancellationToken ct)
    {
        var deleted = await mediator.Send(new DeleteToadPlanSqlCommand(id), ct);
        return deleted ? Results.NoContent() : Results.NotFound();
    }
}
