namespace DeductionService.Application.DTOs;

public record AdhocPayDeductionDto(
    long SystemId,
    long? CanteenUnit,
    long? SerialNumber,
    long? BatchNumber,
    DateTime? TransactionDate,
    string? EarningDeductionCode,
    double? ReferenceNumber,
    decimal? PayAmount,
    long? OppositeAmount,
    DateTime? EntryDate,
    long? EnteredByUserId,
    string? CancelFlag,
    long? AttachmentNumber,
    string? CompanyCode,
    long? EmployeeNumber,
    string? UpdateFlag,
    long? SequenceNumber,
    string? GradeType);

public record AdhocPayDeductionHistoryDto(
    long SystemId,
    long CanteenUnit,
    long? SerialNumber,
    long? BatchNumber,
    DateTime? TransactionDate,
    string? EarningDeductionCode,
    decimal? PayAmount,
    DateTime? EntryDate,
    long? EnteredByUserId,
    string? CancelFlag,
    string? CompanyCode,
    long? EmployeeNumber);

public record DeductionAccessDto(
    long? AccessNumber,
    long? UnitCode,
    string? DeductionType,
    decimal? SystemId,
    decimal? EnteredByUserId,
    DateTime? EnteredOn,
    DateTime? ClosedOn,
    bool IsActive);

public record DeductionAmountDto(
    decimal EmployeeShare,
    decimal EmployerShare,
    decimal Total);

public record ProcessMonthlyDeductionResultDto(
    string MonthYear,
    int ProcessedCount,
    decimal TotalAmount,
    bool Success,
    string? ErrorMessage);

public record CreateAdhocDeductionDto(
    long SystemId,
    long? CanteenUnit,
    decimal? PayAmount,
    string? EarningDeductionCode,
    long? EmployeeNumber,
    long EnteredByUserId,
    string? CompanyCode,
    string? GradeType);
