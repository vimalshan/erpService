using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.Application.Queries.Attendance;

public record GetAttendancePercentageQuery(long EmpSysId, DateTime MonthStart, DateTime MonthEnd)
    : IRequest<AttendancePercentageDto>;
