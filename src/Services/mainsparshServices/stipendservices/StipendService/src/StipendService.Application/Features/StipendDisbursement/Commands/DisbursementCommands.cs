using MediatR;
using StipendService.Application.DTOs;

namespace StipendService.Application.Features.StipendDisbursement.Commands;

public record ProcessMonthlyStipendCommand(
    string MonthYear,
    long ProcessedBy
) : IRequest<ProcessMonthlyStipendResultDto>;

public record CalculateAndDisburseStipendCommand(
    string MonthYear,
    long ProcessedBy
) : IRequest<CalculateDisbursementResultDto>;

public record RejectDisbursementCommand(
    long DisbursementId,
    long UpdatedBy
) : IRequest<bool>;

public record SetBankReferenceCommand(
    long DisbursementId,
    string BankReference,
    string ReferenceNo,
    long UpdatedBy
) : IRequest<StipendDisbursementDto>;
