using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.Application.Commands.Overtime;

public record ApproveOvertimeCommand(long OvertimeId, long ApprovedBy) : IRequest<OvertimeDto>;
