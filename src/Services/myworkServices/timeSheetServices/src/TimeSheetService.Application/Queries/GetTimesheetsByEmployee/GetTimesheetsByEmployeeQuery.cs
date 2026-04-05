using MediatR;
using TimeSheetService.Application.DTOs;

namespace TimeSheetService.Application.Queries.GetTimesheetsByEmployee;

public record GetTimesheetsByEmployeeQuery(long EmployeeSysId, DateTime? From = null, DateTime? To = null)
    : IRequest<IEnumerable<TimesheetEntryDto>>;
