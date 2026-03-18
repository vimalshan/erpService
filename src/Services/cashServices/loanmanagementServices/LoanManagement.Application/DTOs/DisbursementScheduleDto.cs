namespace LoanManagement.Application.DTOs;

public record DisbursementScheduleDto(
    long DisbId,
    decimal? DisbLoanId,
    DateTime? DisbDate,
    decimal? DisbAmount,
    decimal? DisbExcRate,
    decimal? DisbExcAmt
);
