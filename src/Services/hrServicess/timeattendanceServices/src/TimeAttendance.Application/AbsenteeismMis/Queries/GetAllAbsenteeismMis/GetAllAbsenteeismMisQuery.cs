using MediatR;
using TimeAttendance.Application.DTOs;

namespace TimeAttendance.Application.AbsenteeismMis.Queries.GetAllAbsenteeismMis;

public record GetAllAbsenteeismMisQuery(
    int PageNumber = 1,
    int PageSize = 20,
    int? UnitId = null,
    string? Month = null
) : IRequest<PaginatedResult<AbsenteeismMisDto>>;
