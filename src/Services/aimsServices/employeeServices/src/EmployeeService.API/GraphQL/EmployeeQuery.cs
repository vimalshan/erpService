using HotChocolate;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.API.GraphQL;

public sealed class EmployeeQuery
{
    public async Task<IEnumerable<EmployeeTimeInfoDto>> GetTimeInfos(
        [Service] IEmployeeTimeInfoRepository repo,
        long empSysId,
        CancellationToken ct)
    {
        var items = await repo.GetByEmployeeIdAsync(empSysId, ct);
        return items.Select(i => new EmployeeTimeInfoDto(
            i.TimeInfoId, i.EmpSysId.Value, i.EmpAttFlag.Value, i.LastModifiedBy, i.LastModifiedOn));
    }

    public async Task<IEnumerable<EmployeeApproverDto>> GetApprovers(
        [Service] IEmployeeApproverRepository repo,
        long empSysId,
        CancellationToken ct)
    {
        var items = await repo.GetByEmployeeIdAsync(empSysId, ct);
        return items.Select(a => new EmployeeApproverDto(
            a.ApproverId, a.EmpSysId.Value, a.Level.Value,
            a.ApproverSysId, a.EffDate, a.LastModifiedBy, a.LastModifiedOn));
    }

    public async Task<IEnumerable<EmployeeCalendarDto>> GetCalendars(
        [Service] IEmployeeCalendarRepository repo,
        long empSysId,
        CancellationToken ct)
    {
        var items = await repo.GetByEmployeeIdAsync(empSysId, ct);
        return items.Select(c => new EmployeeCalendarDto(
            c.EmpCalId, c.EmpSysId.Value, c.CalendarId, c.SwipeId,
            c.EffDate, c.ClsDate, c.Status, c.Transfer, c.SettlementNo,
            c.LastModifiedBy, c.LastModifiedOn));
    }
}
