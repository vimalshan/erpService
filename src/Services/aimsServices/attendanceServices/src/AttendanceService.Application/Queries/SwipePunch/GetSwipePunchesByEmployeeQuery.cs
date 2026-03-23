using AttendanceService.Application.DTOs;
using MediatR;

namespace AttendanceService.Application.Queries.SwipePunch;

public record GetSwipePunchesByEmployeeQuery(long EmpSysId, DateTime? From, DateTime? To)
    : IRequest<IEnumerable<SwipePunchDto>>;
