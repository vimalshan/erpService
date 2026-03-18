using MediatR;
using EmployeeService.Application.DTOs;

namespace EmployeeService.Application.Commands.MapCalendar;

public record MapCalendarCommand(
    long EmpSysId,
    int CalendarId,
    long MappedBy
) : IRequest<EmployeeCalendarDto>;
