using MediatR;
using CashManagement.Application.Commands.CashUnit;
using CashManagement.Application.Commands.CashTransaction;
using CashManagement.Application.Commands.BankAccount;
using CashManagement.Application.Commands.BankTransaction;
using CashManagement.Application.Commands.ChequeRegister;
using CashManagement.Application.Commands.BankReconciliation;
using CashManagement.Application.DTOs;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.API.GraphQL;

public class Mutation
{
    public async Task<CashUnitDto> CreateCashUnit([Service] IMediator mediator, CreateCashUnitInput input, CancellationToken ct)
        => await mediator.Send(new CreateCashUnitCommand(
            input.CashUnitId, input.Name, input.Code, input.Location,
            input.InChargeEmployeeId, input.OpeningBalance, input.CreatedBy), ct);

    public async Task<CashTransactionDto> RecordCashReceipt([Service] IMediator mediator, RecordCashReceiptInput input, CancellationToken ct)
        => await mediator.Send(new RecordCashReceiptCommand(
            input.CashUnitId, input.Amount, input.Source, input.RefNo, input.Remarks, input.CreatedBy), ct);

    public async Task<CashTransactionDto> RecordCashDisbursement([Service] IMediator mediator, RecordCashDisbursementInput input, CancellationToken ct)
        => await mediator.Send(new RecordCashDisbursementCommand(
            input.CashUnitId, input.Amount, input.Source, input.PayeeId, input.RefNo, input.Remarks, input.CreatedBy), ct);

    public async Task<BankAccountDto> CreateBankAccount([Service] IMediator mediator, CreateBankAccountInput input, CancellationToken ct)
        => await mediator.Send(new CreateBankAccountCommand(
            input.BankAccountId, input.BankName, input.AccountNo, input.Branch, input.AccountType, input.CreatedBy), ct);

    public async Task<BankTransactionDto> RecordBankTransaction([Service] IMediator mediator, RecordBankTransactionInput input, CancellationToken ct)
        => await mediator.Send(new RecordBankTransactionCommand(
            input.BankAccountId, input.TxnType, input.Amount, input.Reference, input.Remarks, input.CreatedBy), ct);

    public async Task<ChequeDto> IssueCheque([Service] IMediator mediator, IssueChequeInput input, CancellationToken ct)
        => await mediator.Send(new IssueChequeCommand(
            input.BankAccountId, input.ChequeNumber, input.PayeeName,
            input.Amount, input.ChequeDate, input.Reference, input.IssuedBy), ct);

    public async Task<bool> MarkChequeBounced([Service] IMediator mediator, long chequeId, string bounceReason, long processedBy, CancellationToken ct)
        => await mediator.Send(new MarkChequeBouncedCommand(chequeId, bounceReason, processedBy), ct);

    public async Task<bool> MarkChequeCleared([Service] IMediator mediator, long chequeId, long processedBy, CancellationToken ct)
        => await mediator.Send(new MarkChequeClearedCommand(chequeId, processedBy), ct);

    public async Task<BankReconciliationDto> PerformReconciliation([Service] IMediator mediator, PerformReconciliationInput input, CancellationToken ct)
        => await mediator.Send(new PerformBankReconciliationCommand(
            input.BankAccountId, input.BankStatementBalance, input.ReconciliationDate, input.CreatedBy), ct);
}

// Input types for GraphQL mutations
public record CreateCashUnitInput(long CashUnitId, string Name, string Code, string? Location,
    long? InChargeEmployeeId, decimal OpeningBalance, long CreatedBy);
public record RecordCashReceiptInput(long CashUnitId, decimal Amount, string? Source, string? RefNo, string? Remarks, long CreatedBy);
public record RecordCashDisbursementInput(long CashUnitId, decimal Amount, string? Source, long? PayeeId, string? RefNo, string? Remarks, long CreatedBy);
public record CreateBankAccountInput(long BankAccountId, string BankName, string AccountNo, string? Branch, string? AccountType, long CreatedBy);
public record RecordBankTransactionInput(long BankAccountId, BankTransactionType TxnType, decimal Amount, string? Reference, string? Remarks, long CreatedBy);
public record IssueChequeInput(long BankAccountId, string ChequeNumber, string PayeeName, decimal Amount, DateOnly ChequeDate, string? Reference, long IssuedBy);
public record PerformReconciliationInput(long BankAccountId, decimal BankStatementBalance, DateOnly ReconciliationDate, long CreatedBy);
