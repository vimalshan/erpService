using ReimbursementService.Domain.Enums;

namespace ReimbursementService.Application.DTOs;

public sealed record ReimbursementDto(
    long ReimId,
    string ReimRefNo,
    long EmpSysId,
    string ReimType,
    decimal ReimAmount,
    string ReimCurrency,
    DateOnly ReimDate,
    DateOnly ExpenseDate,
    string? Description,
    string? Location,
    string Status,
    int? ApprovalLevel,
    long? ApprovedBy,
    DateTime? ApprovedOn,
    string? RejectionReason,
    DateOnly? PaymentDate,
    long CreatedBy,
    DateTime CreatedOn,
    long? UpdatedBy,
    DateTime? UpdatedOn
);

public sealed record ReimbursementSummaryDto(
    long EmpSysId,
    string ReimType,
    int Count,
    decimal TotalAmount,
    string Currency
);

public sealed record CreateReimbursementRequestDto(
    long EmpSysId,
    string ReimType,
    decimal Amount,
    string Currency,
    DateOnly ReimDate,
    DateOnly ExpenseDate,
    string? Description,
    string? Location
);

public sealed record UpdateReimbursementRequestDto(
    string ReimType,
    decimal Amount,
    string Currency,
    DateOnly ReimDate,
    DateOnly ExpenseDate,
    string? Description,
    string? Location
);

public sealed record ApproveReimbursementRequestDto(
    long ApprovedBy,
    int ApprovalLevel
);

public sealed record RejectReimbursementRequestDto(
    long RejectedBy,
    string Reason
);

public sealed record MarkAsPaidRequestDto(
    DateOnly PaymentDate,
    long UpdatedBy
);
