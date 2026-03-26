using MediatR;
using CanteenTransactionService.Application.DTOs;

namespace CanteenTransactionService.Application.CQRS.Commands;

// ---- CanteenDacon Commands ----

public record RecordCanteenTransactionCommand(
    long CompanyCode,
    long EmployeeSysId,
    string EmployeeType,
    string SwipeDate,
    long ItemCode,
    string ItemType,
    decimal EmployeeContribution,
    decimal EmployerContribution,
    string? CanteenNumber,
    long ItemQuantity,
    long EntryUser,
    string? GradeCategory) : IRequest<CanteenDaconDto>;

public record CancelCanteenTransactionCommand(
    long SerialNumber) : IRequest<bool>;

// ---- DailyAvailed Commands ----

public record ProcessDailyAvailedCommand(
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
    string? GradeCategory) : IRequest<DailyAvailedDto>;

// ---- MIS Batch Commands ----

public record SubmitMisBatchCommand(
    long CompanyCode,
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long ItemQuantity,
    DateTime BatchDate,
    long BatchNumber,
    string CanteenNumber,
    string GateNumber) : IRequest<MisBatchSubmissionDto>;

public record ProcessMisBatchCommand(
    long SerialNumber) : IRequest<bool>;

public record FailMisBatchCommand(
    long SerialNumber) : IRequest<bool>;
