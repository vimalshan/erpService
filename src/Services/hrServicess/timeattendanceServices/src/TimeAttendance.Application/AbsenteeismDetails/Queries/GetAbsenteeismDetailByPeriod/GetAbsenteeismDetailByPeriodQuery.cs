using MediatR;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetailByPeriod;

public record GetAbsenteeismDetailByPeriodQuery(
    long UnitId,
    int Year,
    int Month
) : IRequest<IEnumerable<AbsenteeismDetailDto>>;
