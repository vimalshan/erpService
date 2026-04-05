using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Application.Features.TransactionLogs.Commands;
using TransactionService.Application.Features.TransactionLogs.Queries;

namespace TransactionService.API.Endpoints;

public static class TransactionLogEndpoints
{
    public static WebApplication MapTransactionLogEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minimal/logs").WithTags("Minimal - Transaction Logs");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAllTransactionLogsQuery(), ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTransactionLogByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).RequireAuthorization();

        group.MapGet("/by-entity", async (string transactionType, long transactionId, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTransactionLogsByEntityQuery(transactionType, transactionId), ct);
            return Results.Ok(result);
        }).RequireAuthorization();

        group.MapPost("/", async (LogTransactionCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/logs/{result.LogId}", result);
        }).RequireAuthorization();

        return app;
    }
}
