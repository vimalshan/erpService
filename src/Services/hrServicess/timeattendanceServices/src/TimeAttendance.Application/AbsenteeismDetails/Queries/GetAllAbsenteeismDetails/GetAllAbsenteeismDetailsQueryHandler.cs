using MediatR;
using TimeAttendance.Application.DTOs;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismDetails.Queries.GetAllAbsenteeismDetails;

public class GetAllAbsenteeismDetailsQueryHandler(IAbsenteeismDetailRepository repository)
    : IRequestHandler<GetAllAbsenteeismDetailsQuery, PaginatedResult<AbsenteeismDetailDto>>
{
    public async Task<PaginatedResult<AbsenteeismDetailDto>> Handle(
        GetAllAbsenteeismDetailsQuery request, CancellationToken cancellationToken)
    {
        var all = request.UnitId.HasValue
            ? await repository.GetByUnitAsync(request.UnitId.Value, cancellationToken)
            : await repository.GetAllAsync(cancellationToken);

        var list = all.ToList();
        var total = list.Count;

        var items = list
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new AbsenteeismDetailDto(
                e.Id, e.UnitId, e.Year, e.Month,
                e.TotalManDays, e.AbsentManDays,
                e.TotalManDays - e.AbsentManDays,
                e.AbsenteeismRate,
                e.GradeCategory, e.FunctionId, e.AgeId,
                e.ExperienceId, e.Gender.ToString(),
                e.InternalExperienceId, e.TotalExperienceId,
                e.CreatedAt, e.CreatedBy,
                e.LastModifiedAt, e.LastModifiedBy))
            .ToList();

        return new PaginatedResult<AbsenteeismDetailDto>(
            items, total, request.PageNumber, request.PageSize);
    }
}
