namespace AccountingService.Application.DTOs;

public record TransactionDetailDto(
    string TdTrustCode,
    int TransactionId,
    string TdTransactionCode,
    string? TdTransactionType,
    DateTime TdTransactionDate,
    decimal TdAmount,
    string? TdRemarks,
    int? TdMemberNo,
    string TdTypeCode,
    long TdFinyear,
    string TdJvVoucherType,
    string TdJvNo,
    bool IsCancelled
);
