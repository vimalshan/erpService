using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Attendance.Queries.GetAttendanceSummary;

public sealed record GetAttendanceSummaryQuery(
    long EmployeeSysId,
    DateTime MonthStart,
    DateTime MonthEnd) : IRequest<AttendanceSummaryDto?>;
