using MediatR;
using CashManagement.Application.DTOs;

namespace CashManagement.Application.Commands.CashTransaction;

public record RecordCashReceiptCommand(
    long CashUnitId,
    decimal Amount,
    string? Source,
    string? RefNo,
    string? Remarks,
    long CreatedBy,
    long? AuthorizedBy = null
) : IRequest<CashTransactionDto>;

public record RecordCashDisbursementCommand(
    long CashUnitId,
    decimal Amount,
    string? Source,
    long? PayeeId,
    string? RefNo,
    string? Remarks,
    long CreatedBy,
    long? AuthorizedBy = null
) : IRequest<CashTransactionDto>;
