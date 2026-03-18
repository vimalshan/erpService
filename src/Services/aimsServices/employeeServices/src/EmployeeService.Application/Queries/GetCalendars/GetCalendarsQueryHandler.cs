using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Queries.GetCalendars;

public sealed class GetCalendarsByEmployeeQueryHandler
    : IRequestHandler<GetCalendarsByEmployeeQuery, IEnumerable<EmployeeCalendarDto>>
{
    private readonly IEmployeeCalendarRepository _repo;
    public GetCalendarsByEmployeeQueryHandler(IEmployeeCalendarRepository repo) => _repo = repo;

    public async Task<IEnumerable<EmployeeCalendarDto>> Handle(GetCalendarsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByEmployeeIdAsync(request.EmpSysId, cancellationToken);
        return items.Select(c => new EmployeeCalendarDto(
            c.EmpCalId, c.EmpSysId.Value, c.CalendarId, c.SwipeId,
            c.EffDate, c.ClsDate, c.Status, c.Transfer, c.SettlementNo,
            c.LastModifiedBy, c.LastModifiedOn));
    }
}
