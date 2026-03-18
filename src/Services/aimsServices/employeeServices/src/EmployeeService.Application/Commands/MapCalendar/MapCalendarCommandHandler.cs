using MediatR;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Interfaces;

namespace EmployeeService.Application.Commands.MapCalendar;

public sealed class MapCalendarCommandHandler : IRequestHandler<MapCalendarCommand, EmployeeCalendarDto>
{
    private readonly IEmployeeCalendarRepository _repo;

    public MapCalendarCommandHandler(IEmployeeCalendarRepository repo) => _repo = repo;

    public async Task<EmployeeCalendarDto> Handle(MapCalendarCommand request, CancellationToken cancellationToken)
    {
        var nextId = await _repo.GetNextIdAsync(cancellationToken);
        var calendar = EmployeeCalendar.Create(nextId, request.EmpSysId, request.CalendarId, request.MappedBy);
        await _repo.AddAsync(calendar, cancellationToken);

        return new EmployeeCalendarDto(
            calendar.EmpCalId,
            calendar.EmpSysId.Value,
            calendar.CalendarId,
            calendar.SwipeId,
            calendar.EffDate,
            calendar.ClsDate,
            calendar.Status,
            calendar.Transfer,
            calendar.SettlementNo,
            calendar.LastModifiedBy,
            calendar.LastModifiedOn
        );
    }
}
