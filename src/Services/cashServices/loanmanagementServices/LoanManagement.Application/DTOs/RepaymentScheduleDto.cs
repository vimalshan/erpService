namespace LoanManagement.Application.DTOs;

public record RepaymentScheduleDto(
    long RepayId,
    decimal? RepayLoanId,
    DateTime? RepayDate,
    decimal? RepayAmt,
    string? RepayFlag
);
