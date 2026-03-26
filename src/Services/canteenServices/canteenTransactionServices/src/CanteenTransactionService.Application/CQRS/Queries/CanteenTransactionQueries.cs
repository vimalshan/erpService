using MediatR;
using CanteenTransactionService.Application.DTOs;

namespace CanteenTransactionService.Application.CQRS.Queries;

// ---- CanteenDacon Queries ----

public record GetTransactionBySerialNumberQuery(
    long SerialNumber) : IRequest<CanteenDaconDto?>;

public record GetTransactionsByEmployeeQuery(
    long EmployeeSysId,
    string FromDate,
    string ToDate) : IRequest<IEnumerable<CanteenDaconDto>>;

public record GetTransactionsByCompanyAndDateQuery(
    long CompanyCode,
    string SwipeDate) : IRequest<IEnumerable<CanteenDaconDto>>;

// ---- DailyAvailed Queries ----

public record GetDailyAvailedBySerialNumberQuery(
    long SerialNumber) : IRequest<DailyAvailedDto?>;

public record GetDailyAvailedByEmployeeQuery(
    long EmployeeSysId,
    string FromDate,
    string ToDate) : IRequest<IEnumerable<DailyAvailedDto>>;

public record GetDailyAvailedByCompanyAndDateQuery(
    long CompanyCode,
    string SwipeDate) : IRequest<IEnumerable<DailyAvailedDto>>;

// ---- MIS Batch Queries ----

public record GetMisBatchBySerialNumberQuery(
    long SerialNumber) : IRequest<MisBatchSubmissionDto?>;

public record GetMisBatchByBatchNumberQuery(
    long BatchNumber) : IRequest<IEnumerable<MisBatchSubmissionDto>>;

public record GetPendingMisBatchesQuery() : IRequest<IEnumerable<MisBatchSubmissionDto>>;
