using MediatR;
using TransactionProcessing.Application.Commands;
using TransactionProcessing.Application.Queries;

namespace TransactionProcessing.API.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/minimal/transactions")
            .WithTags("Transactions (Minimal)")
            .RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTransactionByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        }).WithName("GetTransactionMinimal");

        group.MapGet("/ledger", async (
            DateTime? from, DateTime? to, string? status, string? txnType,
            int? pageSize, int? page, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetTransactionLedgerQuery(from, to, status, txnType, pageSize ?? 50, page ?? 1), ct);
            return Results.Ok(result);
        }).WithName("GetLedgerMinimal");

        group.MapPost("/cash-transfer", async (ProcessCashTransferCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created($"/api/minimal/transactions/{result.TxnId}", result);
        }).WithName("CashTransferMinimal");

        group.MapPost("/settlements", async (ProcessDealSettlementCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created("", result);
        }).WithName("SettlementMinimal");

        group.MapPost("/disbursements", async (ProcessLoanDisbursementCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created("", result);
        }).WithName("DisbursementMinimal");

        group.MapPost("/repayments", async (ProcessLoanRepaymentCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);
            return Results.Created("", result);
        }).WithName("RepaymentMinimal");
    }
}
