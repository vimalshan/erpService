using CanteenTransactionService.Application.DTOs;
using CanteenTransactionService.Application.CQRS.Queries;
using CanteenTransactionService.Application.CQRS.Commands;
using CanteenTransactionService.Infrastructure.Persistence.Dapper;
using MediatR;

namespace CanteenTransactionService.API.GraphQL;

public class CanteenTransactionQuery
{
    public async Task<IEnumerable<CanteenDaconDto>> GetTransactionsByEmployeeAsync(
        long employeeSysId,
        string fromDate,
        string toDate,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetTransactionsByEmployeeQuery(employeeSysId, fromDate, toDate), ct);

    public async Task<CanteenDaconDto?> GetTransactionAsync(
        long serialNumber,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetTransactionBySerialNumberQuery(serialNumber), ct);

    public async Task<IEnumerable<DailyAvailedDto>> GetDailyAvailedByEmployeeAsync(
        long employeeSysId,
        string fromDate,
        string toDate,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetDailyAvailedByEmployeeQuery(employeeSysId, fromDate, toDate), ct);

    public async Task<IEnumerable<MisBatchSubmissionDto>> GetPendingBatchesAsync(
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new GetPendingMisBatchesQuery(), ct);

    public async Task<IEnumerable<TransactionSummaryDto>> GetDailySummaryAsync(
        long companyCode,
        string swipeDate,
        [Service] TransactionDapperRepository dapperRepo) =>
        await dapperRepo.GetDailySummaryAsync(companyCode, swipeDate);
}

public class CanteenTransactionMutation
{
    public async Task<CanteenDaconDto> RecordTransactionAsync(
        RecordTransactionInput input,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new RecordCanteenTransactionCommand(
            input.CompanyCode, input.EmployeeSysId, input.EmployeeType,
            input.SwipeDate, input.ItemCode, input.ItemType,
            input.EmployeeContribution, input.EmployerContribution,
            input.CanteenNumber, input.ItemQuantity, input.EntryUser,
            input.GradeCategory), ct);

    public async Task<DailyAvailedDto> ProcessDailyAvailedAsync(
        ProcessDailyAvailedInput input,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new ProcessDailyAvailedCommand(
            input.CompanyCode, input.EmployeeSysId, input.EmployeeType,
            input.SwipeDate, input.ItemCode, input.ItemType,
            input.EmployeeContribution, input.EmployerContribution,
            input.CanteenNumber, input.ItemQuantity, input.EntryUser,
            input.GradeCategory), ct);

    public async Task<MisBatchSubmissionDto> SubmitBatchAsync(
        SubmitBatchInput input,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new SubmitMisBatchCommand(
            input.CompanyCode, input.EmployeeNumber, input.SwipeTime,
            input.ItemCode, input.ItemQuantity, input.BatchDate,
            input.BatchNumber, input.CanteenNumber, input.GateNumber), ct);

    public async Task<bool> CancelTransactionAsync(
        long serialNumber,
        [Service] IMediator mediator,
        CancellationToken ct) =>
        await mediator.Send(new CancelCanteenTransactionCommand(serialNumber), ct);
}

public record RecordTransactionInput(
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
    string? GradeCategory);

public record ProcessDailyAvailedInput(
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
    string? GradeCategory);

public record SubmitBatchInput(
    long CompanyCode,
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long ItemQuantity,
    DateTime BatchDate,
    long BatchNumber,
    string CanteenNumber,
    string GateNumber);
