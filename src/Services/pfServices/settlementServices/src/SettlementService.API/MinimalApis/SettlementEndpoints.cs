using MediatR;
using Microsoft.AspNetCore.Mvc;
using SettlementService.Application.Commands.CreateSettlement;
using SettlementService.Application.DTOs;
using SettlementService.Application.Queries.GetSettlement;
using SettlementService.Application.Queries.GetSettlements;
using SettlementService.Application.Queries.GetSettlementsByMember;

namespace SettlementService.API.MinimalApis;

public static class SettlementEndpoints
{
    public static void MapSettlementEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/settlements")
            .WithTags("Settlements (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSettlementsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllSettlementsV2")
        .Produces<IEnumerable<SettlementDto>>();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSettlementQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetSettlementByIdV2")
        .Produces<SettlementDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/member/{memberNo:long}", async (long memberNo, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSettlementsByMemberQuery(memberNo), ct);
            return Results.Ok(result);
        })
        .WithName("GetSettlementsByMemberV2")
        .Produces<IEnumerable<SettlementDto>>();

        group.MapPost("/", async ([FromBody] CreateSettlementCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/settlements/{result.SettlementNumber}", result);
        })
        .WithName("CreateSettlementV2")
        .Produces<SettlementDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
