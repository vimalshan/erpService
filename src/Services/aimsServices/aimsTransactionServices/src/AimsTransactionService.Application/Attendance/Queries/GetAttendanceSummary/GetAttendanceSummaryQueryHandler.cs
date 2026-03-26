using MediatR;
using AimsTransactionService.Application.Common.Interfaces;
using AimsTransactionService.Application.DTOs;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Application.Attendance.Queries.GetAttendanceSummary;

public sealed class GetAttendanceSummaryQueryHandler(IAttendanceSummaryRepository summaryRepository)
    : IRequestHandler<GetAttendanceSummaryQuery, AttendanceSummaryDto?>
{
    public async Task<AttendanceSummaryDto?> Handle(
        GetAttendanceSummaryQuery request, CancellationToken cancellationToken)
    {
        var summary = await summaryRepository.GetByEmployeeMonthAsync(
            request.EmployeeSysId, request.MonthStart, request.MonthEnd, cancellationToken);

        return summary is null ? null : MapToDto(summary);
    }

    private static AttendanceSummaryDto MapToDto(AttendanceSummary s) => new(
        s.Id,
        s.EmployeeSysId,
        s.MonthStart,
        s.MonthEnd,
        s.WorkingDays,
        s.PresentDays,
        s.AbsentDays,
        s.OvertimeHours,
        s.LopDays);
}
