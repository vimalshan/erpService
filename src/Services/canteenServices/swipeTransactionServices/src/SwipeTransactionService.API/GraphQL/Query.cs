using SwipeTransactionService.Application.DTOs;
using SwipeTransactionService.Domain.Interfaces.Repositories;
using SwipeTransactionService.Infrastructure.Dapper;

namespace SwipeTransactionService.API.GraphQL;

public sealed class Query
{
    public async Task<IEnumerable<SwipeCardUploadDto>> GetSwipesByEmployee(
        string employeeNumber,
        DateTime from,
        DateTime to,
        [Service] ISwipeCardUploadRepository repository,
        CancellationToken ct)
    {
        var entities = await repository.GetByEmployeeAsync(employeeNumber, from, to, ct);
        return entities.Select(e => new SwipeCardUploadDto(
            e.CompanyCode, e.EmployeeNumber, e.SwipeTime, e.ItemCode, e.ItemQuantity,
            e.BatchNumber, e.SerialNumber, e.BatchDate, e.EntryDate,
            e.CanteenNumber, e.GateNumber, e.UpdateStatus, e.FlexField1, e.FlexField2));
    }

    public async Task<IEnumerable<SwipeUploadSummaryDto>> GetBatchSummary(
        long batchNumber,
        [Service] SwipeReportQueryService queryService,
        CancellationToken ct)
        => await queryService.GetSummaryByBatchAsync(batchNumber, ct);

    public async Task<IEnumerable<DailyAvailedDto>> GetDailyAvailed(
        long empSysId,
        string date,
        [Service] SwipeReportQueryService queryService,
        CancellationToken ct)
        => await queryService.GetDailyAvailedByEmployeeAsync(empSysId, date, ct);

    public async Task<CanteenPunchDto?> GetTodayPunch(
        long empSysId,
        [Service] ICanteenPunchRepository repository,
        CancellationToken ct)
    {
        var entity = await repository.GetByEmployeeAndDateAsync(empSysId, DateTime.UtcNow.Date, ct);
        if (entity is null) return null;
        return new CanteenPunchDto(
            entity.SerialNumber, entity.CompanyCode, entity.EmployeeSysId,
            entity.CanteenUnit, entity.PunchDate, entity.TimeIn, entity.TimeOut, entity.WorkHours);
    }
}
