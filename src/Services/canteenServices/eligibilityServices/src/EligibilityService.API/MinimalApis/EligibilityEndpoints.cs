using MediatR;
using EligibilityService.Application.Commands.EligibilityMaster;
using EligibilityService.Application.DTOs;
using EligibilityService.Application.Queries.EligibilityMaster;
using Microsoft.AspNetCore.Mvc;

namespace EligibilityService.API.MinimalApis;

public static class EligibilityEndpoints
{
    public static IEndpointRouteBuilder MapEligibilityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/eligibility/v2")
            .WithTags("Eligibility Minimal API")
            .RequireAuthorization();

        group.MapGet("/", async (IMediator mediator, [FromQuery] long? canteenUnit, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllEligibilityMastersQuery(canteenUnit), ct);
            return Results.Ok(result);
        }).WithName("GetAllEligibility");

        group.MapGet("/{canteenUnit:long}/{shiftCode}/{itemCode:decimal}",
            async (IMediator mediator, long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct) =>
            {
                var result = await mediator.Send(new GetEligibilityMasterQuery(canteenUnit, shiftCode, itemCode), ct);
                return result is null ? Results.NotFound() : Results.Ok(result);
            }).WithName("GetEligibilityById");

        group.MapPost("/", async (IMediator mediator, [FromBody] CreateEligibilityMasterCommand cmd, CancellationToken ct) =>
        {
            var result = await mediator.Send(cmd, ct);
            return Results.Created($"/eligibility/v2/{result.CanteenUnit}/{result.ShiftCode}/{result.ItemCode}", result);
        }).WithName("CreateEligibility");

        group.MapPut("/{canteenUnit:long}/{shiftCode}/{itemCode:decimal}",
            async (IMediator mediator, long canteenUnit, string shiftCode, decimal itemCode,
                   [FromBody] UpdateEligibilityMasterCommand cmd, CancellationToken ct) =>
            {
                var result = await mediator.Send(cmd, ct);
                return Results.Ok(result);
            }).WithName("UpdateEligibility");

        group.MapDelete("/{canteenUnit:long}/{shiftCode}/{itemCode:decimal}",
            async (IMediator mediator, long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct) =>
            {
                await mediator.Send(new DeleteEligibilityMasterCommand(canteenUnit, shiftCode, itemCode), ct);
                return Results.NoContent();
            }).WithName("DeleteEligibility");

        return app;
    }
}
