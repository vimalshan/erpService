using MediatR;
using CanteenTransactionService.Application.CQRS.Commands;
using CanteenTransactionService.Application.CQRS.Queries;

namespace CanteenTransactionService.API.MinimalApis;

public static class CanteenTransactionMinimalApi
{
    public static IEndpointRouteBuilder MapCanteenTransactionMinimalApis(this IEndpointRouteBuilder app)
    {
        // ---- Canteen Transactions ----
        var txnGroup = app.MapGroup("/api/v2/canteen-transactions")
            .WithTags("CanteenTransactions-Minimal")
            .RequireAuthorization();

        txnGroup.MapPost("/", async (RecordCanteenTransactionCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/canteen-transactions/{result.SerialNumber}", result);
        })
        .WithName("RecordTransactionMinimal")
        .WithSummary("Record a canteen meal transaction");

        txnGroup.MapGet("/{serialNumber:long}", async (long serialNumber, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTransactionBySerialNumberQuery(serialNumber), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        })
        .WithName("GetTransactionMinimal");

        txnGroup.MapGet("/employee/{employeeSysId:long}", async (long employeeSysId, string fromDate, string toDate, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTransactionsByEmployeeQuery(employeeSysId, fromDate, toDate), ct);
            return Results.Ok(result);
        })
        .WithName("GetTransactionsByEmployeeMinimal");

        txnGroup.MapDelete("/{serialNumber:long}", async (long serialNumber, IMediator mediator, CancellationToken ct) =>
        {
            await mediator.Send(new CancelCanteenTransactionCommand(serialNumber), ct);
            return Results.NoContent();
        })
        .WithName("CancelTransactionMinimal");

        // ---- Daily Availed ----
        var avlGroup = app.MapGroup("/api/v2/daily-availed")
            .WithTags("DailyAvailed-Minimal")
            .RequireAuthorization();

        avlGroup.MapPost("/", async (ProcessDailyAvailedCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/daily-availed/{result.SerialNumber}", result);
        })
        .WithName("ProcessDailyAvailedMinimal");

        avlGroup.MapGet("/employee/{employeeSysId:long}", async (long employeeSysId, string fromDate, string toDate, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDailyAvailedByEmployeeQuery(employeeSysId, fromDate, toDate), ct);
            return Results.Ok(result);
        })
        .WithName("GetDailyAvailedByEmployeeMinimal");

        // ---- MIS Batch ----
        var batchGroup = app.MapGroup("/api/v2/mis-batch")
            .WithTags("MISBatch-Minimal")
            .RequireAuthorization();

        batchGroup.MapPost("/", async (SubmitMisBatchCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/v2/mis-batch/{result.SerialNumber}", result);
        })
        .WithName("SubmitBatchMinimal");

        batchGroup.MapGet("/pending", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPendingMisBatchesQuery(), ct);
            return Results.Ok(result);
        })
        .WithName("GetPendingBatchesMinimal");

        return app;
    }
}
