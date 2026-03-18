using MediatR;
using TimeAttendance.Application.DTOs;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetailByPeriod;

public class GetAbsenteeismDetailByPeriodQueryHandler(IAbsenteeismDetailRepository repository)
    : IRequestHandler<GetAbsenteeismDetailByPeriodQuery, IEnumerable<AbsenteeismDetailDto>>
{
    public async Task<IEnumerable<AbsenteeismDetailDto>> Handle(
        GetAbsenteeismDetailByPeriodQuery request, CancellationToken cancellationToken)
    {
        var entities = await repository.GetByUnitAndPeriodAsync(
            request.UnitId, request.Year, request.Month, cancellationToken);

        return entities.Select(e => new AbsenteeismDetailDto(
            e.Id, e.UnitId, e.Year, e.Month,
            e.TotalManDays, e.AbsentManDays,
            e.TotalManDays - e.AbsentManDays,
            e.AbsenteeismRate,
            e.GradeCategory, e.FunctionId, e.AgeId,
            e.ExperienceId, e.Gender,
            e.InternalExperienceId, e.TotalExperienceId,
            e.CreatedAt, e.CreatedBy,
            e.LastModifiedAt, e.LastModifiedBy));
    }
}
