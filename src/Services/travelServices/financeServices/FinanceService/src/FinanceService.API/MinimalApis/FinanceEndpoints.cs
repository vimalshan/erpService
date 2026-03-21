using FinanceService.Application.DTOs;
using FinanceService.Application.Features.Batches.Queries.GetAllBatches;
using FinanceService.Application.Features.Invoices.Queries.GetAllInvoices;
using FinanceService.Application.Features.Payments.Queries.GetPaymentDetails;
using MediatR;

namespace FinanceService.API.MinimalApis;

public static class FinanceEndpoints
{
    public static void MapFinanceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v2/finance")
            .WithTags("Finance v2 (Minimal API)")
            .RequireAuthorization();

        group.MapGet("/invoices", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllInvoicesQuery(), ct);
            return Results.Ok(result);
        }).WithName("GetAllInvoicesV2");

        group.MapGet("/batches", async (string? unitCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllBatchesQuery { UnitCode = unitCode }, ct);
            return Results.Ok(result);
        }).WithName("GetAllBatchesV2");

        group.MapGet("/payments", async (string? unitCode, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPaymentDetailsQuery { UnitCode = unitCode }, ct);
            return Results.Ok(result);
        }).WithName("GetAllPaymentsV2");
    }
}
