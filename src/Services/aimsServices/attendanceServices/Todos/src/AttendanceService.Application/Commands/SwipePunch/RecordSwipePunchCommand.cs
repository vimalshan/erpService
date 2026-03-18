using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.Application.Commands.SwipePunch;

public record RecordSwipePunchCommand(
    long EmpSysId,
    DateTime PunchTime,
    string GateNo,
    string PunchStatus) : IRequest<SwipePunchDto>;
