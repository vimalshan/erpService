namespace LoanManagement.Application.DTOs;

public record InterestDto(
    long IntId,
    decimal? IntLoanId,
    string? IntRateType,
    decimal? IntPer,
    long? IntFloatTypeId,
    DateTime? IntEffDate,
    DateTime? IntClsDate
);
