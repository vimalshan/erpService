using MediatR;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.Application.AbsenteeismDetails.Queries.GetAllAbsenteeismDetails;

public record GetAllAbsenteeismDetailsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    long? UnitId = null
) : IRequest<PaginatedResult<AbsenteeismDetailDto>>;
