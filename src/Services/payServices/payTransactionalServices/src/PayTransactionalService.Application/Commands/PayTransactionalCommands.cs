using MediatR;
using PayTransactionalService.Application.Common;
using PayTransactionalService.Application.DTOs;

namespace PayTransactionalService.Application.Commands;

// Pay Transaction commands
public record CreatePayTransactionCommand(CreatePayTransactionDto Detail, string UserId)
    : IRequest<Result<PayTransactionDto>>;
public record CompletePayTransactionCommand(long TransactionId)
    : IRequest<Result<PayTransactionDto>>;
public record RevokePayTransactionCommand(long TransactionId, string RevokedBy, string? Reason = null)
    : IRequest<Result<PayTransactionDto>>;

// Pay Arrear commands
public record CreatePayArrearCommand(CreatePayArrearDto Detail, string UserId)
    : IRequest<Result<PayArrearDto>>;
public record MarkArrearProcessedCommand(long ArrearId)
    : IRequest<Result<PayArrearDto>>;

// Pay Adjustment commands
public record CreatePayAdjustmentCommand(CreatePayAdjustmentDto Detail, string UserId)
    : IRequest<Result<PayAdjustmentDto>>;
public record ApprovePayAdjustmentCommand(long AdjustmentId, long ApprovedBy)
    : IRequest<Result<PayAdjustmentDto>>;
public record RejectPayAdjustmentCommand(long AdjustmentId, long RejectedBy, string? Reason = null)
    : IRequest<Result<PayAdjustmentDto>>;

// Payroll Batch commands
public record ProcessMonthlySalaryCommand(string MonthYear, string UserId)
    : IRequest<Result<PayrollBatchDto>>;
public record RevokePayrollBatchCommand(long BatchId, string RevokedBy)
    : IRequest<Result<PayrollBatchDto>>;
