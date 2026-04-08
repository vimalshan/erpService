using MediatR;
using Microsoft.AspNetCore.Mvc;
using PFTransactionalService.Application.Commands.ProcessContribution;
using PFTransactionalService.Application.DTOs;
using PFTransactionalService.Application.Queries.GetAccumulation;
using PFTransactionalService.Application.Queries.GetAccumulations;
using PFTransactionalService.Application.Queries.GetSettlements;

namespace PFTransactionalService.API.MinimalApis;

public static class PFTransactionEndpoints
{
    public static void MapPFTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/pftransactions")
            .WithTags("PF Transactions (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/accumulations", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccumulationsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllAccumulationsV2")
        .Produces<IEnumerable<PFAccumulationDto>>();

        group.MapGet("/accumulations/{empSysId:long}", async (long empSysId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccumulationQuery(empSysId), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetAccumulationByEmpV2")
        .Produces<PFAccumulationDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/contributions", async ([FromBody] ProcessContributionCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/pftransactions/accumulations/{result.EmpSysId}", result);
        })
        .WithName("ProcessContributionV2")
        .Produces<PFAccumulationDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/settlements", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPFSettlementsQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetAllPFSettlementsV2")
        .Produces<IEnumerable<PFSettlementDto>>();

        group.MapGet("/settlements/employee/{empSysId:long}", async (long empSysId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPFSettlementsByEmpQuery(empSysId), ct);
            return Results.Ok(result);
        })
        .WithName("GetPFSettlementsByEmpV2")
        .Produces<IEnumerable<PFSettlementDto>>();
    }
}
