using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Attendance.Commands.ProcessAttendanceBatch;

public sealed record ProcessAttendanceBatchCommand(
    DateTime MonthStart,
    DateTime MonthEnd,
    long CreatedBy) : IRequest<AttendanceBatchDto>;
