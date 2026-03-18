using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.Application.Commands.AttendanceBatch;

public record ProcessMonthlyAttendanceCommand(
    DateTime MonthStart,
    DateTime MonthEnd,
    long ProcessedBy) : IRequest<AttendanceBatchDto>;
