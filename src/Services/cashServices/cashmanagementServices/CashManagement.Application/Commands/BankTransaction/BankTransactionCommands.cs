using MediatR;
using CashManagement.Application.DTOs;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Application.Commands.BankTransaction;

public record RecordBankTransactionCommand(
    long BankAccountId,
    BankTransactionType TxnType,
    decimal Amount,
    string? Reference,
    string? Remarks,
    long CreatedBy
) : IRequest<BankTransactionDto>;
