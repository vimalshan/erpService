using MediatR;
using TimeAttendance.Application.DTOs;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismDetails.Queries.GetAbsenteeismDetail;

public class GetAbsenteeismDetailQueryHandler(IAbsenteeismDetailRepository repository)
    : IRequestHandler<GetAbsenteeismDetailQuery, AbsenteeismDetailDto?>
{
    public async Task<AbsenteeismDetailDto?> Handle(
        GetAbsenteeismDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null) return null;

        return new AbsenteeismDetailDto(
            entity.Id, entity.UnitId, entity.Year, entity.Month,
            entity.TotalManDays, entity.AbsentManDays,
            entity.TotalManDays - entity.AbsentManDays,
            entity.AbsenteeismRate,
            entity.GradeCategory, entity.FunctionId, entity.AgeId,
            entity.ExperienceId, entity.Gender.ToString(),
            entity.InternalExperienceId, entity.TotalExperienceId,
            entity.CreatedAt, entity.CreatedBy,
            entity.LastModifiedAt, entity.LastModifiedBy);
    }
}
