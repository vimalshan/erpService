namespace Document.Application.DTOs;

public record GeneratedLetterDto(
    decimal? EmployeePin,
    string? EmployeeName,
    string? SignatoryName,
    string? LetterType,
    string? FinalRating,
    DateTime? EffectiveDate,
    DateTime? PrintDate);

public record GenerateLetterRequest(
    decimal? EmployeePin,
    string? EmployeeName,
    string? LetterType,
    DateTime? EffectiveDate,
    string? FinalRating,
    string? SignatoryName,
    string? SignatoryDesignation,
    decimal? AppraisalBasicPay,
    decimal? AppraisalFlexiPay);

public record LetterLogHistoryDto(
    decimal LogSysId,
    string IpAddress,
    DateTime OpenedOn,
    decimal? EmployeeSysId,
    string? LetterType);

public record LogLetterOpenRequest(
    decimal LogSysId,
    string IpAddress,
    decimal? EmployeeSysId,
    string? LetterType,
    decimal? FinancialYearId);
