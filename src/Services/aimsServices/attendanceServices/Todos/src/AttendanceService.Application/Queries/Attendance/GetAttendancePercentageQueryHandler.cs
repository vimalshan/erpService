using AttendanceService.Application.DTOs;
using AttendanceService.Domain.Interfaces;
using MediatR;

namespace AttendanceService.Application.Queries.Attendance;

public class GetAttendancePercentageQueryHandler(ISwipePunchRepository repo)
    : IRequestHandler<GetAttendancePercentageQuery, AttendancePercentageDto>
{
    public async Task<AttendancePercentageDto> Handle(GetAttendancePercentageQuery request, CancellationToken ct)
    {
        var presentDays = await repo.GetDistinctPunchDaysAsync(
            request.EmpSysId, request.MonthStart, request.MonthEnd, ct);

        var workingDays = (int)(request.MonthEnd - request.MonthStart).TotalDays + 1;
        var percentage = workingDays == 0 ? 0m : Math.Round((presentDays * 100m) / workingDays, 2);

        return new AttendancePercentageDto(request.EmpSysId, request.MonthStart,
            request.MonthEnd, presentDays, workingDays, percentage);
    }
}
