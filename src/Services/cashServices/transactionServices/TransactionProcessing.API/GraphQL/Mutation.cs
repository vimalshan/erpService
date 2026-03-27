using MediatR;
using TransactionProcessing.Application.Commands;
using TransactionProcessing.Application.DTOs;

namespace TransactionProcessing.API.GraphQL;

public sealed class Mutation
{
    [GraphQLDescription("Process a deal settlement")]
    public async Task<DealSettlementDto> ProcessDealSettlement(
        ProcessDealSettlementCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Process a loan disbursement")]
    public async Task<LoanDisbursementDto> ProcessLoanDisbursement(
        ProcessLoanDisbursementCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Process a loan repayment")]
    public async Task<LoanRepaymentDto> ProcessLoanRepayment(
        ProcessLoanRepaymentCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Process a cash transfer")]
    public async Task<FinancialTransactionDto> ProcessCashTransfer(
        ProcessCashTransferCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Create a transaction batch")]
    public async Task<TransactionBatchDto> CreateBatch(
        CreateTransactionBatchCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);

    [GraphQLDescription("Complete a transaction batch")]
    public async Task<TransactionBatchDto> CompleteBatch(
        CompleteTransactionBatchCommand input, [Service] IMediator mediator, CancellationToken ct) =>
        await mediator.Send(input, ct);
}
