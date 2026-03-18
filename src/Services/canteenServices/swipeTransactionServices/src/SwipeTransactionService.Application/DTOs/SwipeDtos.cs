namespace SwipeTransactionService.Application.DTOs;

public sealed record SwipeCardUploadDto(
    long CompanyCode,
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long ItemQuantity,
    long BatchNumber,
    long SerialNumber,
    DateTime BatchDate,
    DateTime EntryDate,
    char CanteenNumber,
    string GateNumber,
    char UpdateStatus,
    string? FlexField1,
    string? FlexField2);

public sealed record CanteenPunchDto(
    long? SerialNumber,
    long CompanyCode,
    long EmployeeSysId,
    long CanteenUnit,
    DateTime PunchDate,
    string? TimeIn,
    string? TimeOut,
    decimal? WorkHours);

public sealed record DailyAvailedDto(
    long SerialNumber,
    long CompanyCode,
    long EmployeeSysId,
    long? ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution,
    long? ItemQuantity,
    string? SwipeDate,
    string? CanteenNumber);

public sealed record SwipeUploadSummaryDto(
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long Quantity,
    string Status);
