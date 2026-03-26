namespace CanteenTransactionService.Application.DTOs;

public record CanteenDaconDto(
    long? SerialNumber,
    long? CompanyCode,
    long EmployeeSysId,
    string? EmployeeType,
    string? SwipeDate,
    long? ItemCode,
    string? ItemType,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    string? CanteenNumber,
    long? ItemQuantity,
    long? EntryUser,
    string? EntryDate,
    string? GradeCategory);

public record DailyAvailedDto(
    long SerialNumber,
    long CompanyCode,
    long EmployeeSysId,
    string? EmployeeType,
    string? SwipeDate,
    long? ItemCode,
    string? ItemType,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    string? CanteenNumber,
    long? ItemQuantity,
    long? EntryUser,
    string? EntryDate,
    string? GradeCategory);

public record MisBatchSubmissionDto(
    long CompanyCode,
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long ItemQuantity,
    DateTime BatchDate,
    long BatchNumber,
    long SerialNumber,
    DateTime EntryDate,
    string CanteenNumber,
    string GateNumber,
    string UpdateStatus);

public record TransactionSummaryDto(
    long CompanyCode,
    string SwipeDate,
    int TotalTransactions,
    decimal TotalEmployeeContribution,
    decimal TotalEmployerContribution);
