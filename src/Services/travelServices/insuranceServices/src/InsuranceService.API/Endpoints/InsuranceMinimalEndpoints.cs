using InsuranceService.Application.Commands;
using InsuranceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceService.API.Endpoints;

public static class InsuranceMinimalEndpoints
{
    public static WebApplication MapInsuranceMinimalEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/insurance")
            .WithTags("MinimalApi")
            .RequireAuthorization();

        group.MapGet("/", async (
            [FromQuery] string? companyCode,
            [FromQuery] long? planNumber,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInsuranceDetailsQuery(companyCode, planNumber), ct);
            return Results.Ok(result);
        })
        .WithName("GetInsurancesMinimal");

        group.MapGet("/{companyCode}/{planNumber:long}", async (
            string companyCode,
            long planNumber,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInsuranceDetailsQuery(companyCode, planNumber), ct);
            return result.Count == 0 ? Results.NotFound() : Results.Ok(result[0]);
        })
        .WithName("GetInsuranceByKeyMinimal");

        group.MapPost("/", async (
            RegisterInsuranceCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return result.Success
                ? Results.Created($"/api/minimal/insurance/{command.CompanyCode}/{command.PlanNumber}", result)
                : Results.BadRequest(result);
        })
        .WithName("RegisterInsuranceMinimal");

        group.MapPut("/status", async (
            UpdateInsuranceStatusCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return result.Success ? Results.Ok(result) : Results.NotFound(result);
        })
        .WithName("UpdateInsuranceStatusMinimal");

        return app;
    }
}
