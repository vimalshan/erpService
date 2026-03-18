using CourseService.Application.DTOs;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Schedules.Queries.GetSchedules;

public class GetSchedulesQueryHandler(ICourseScheduleRepository repository) : IRequestHandler<GetSchedulesQuery, IEnumerable<CourseScheduleDto>>
{
    public async Task<IEnumerable<CourseScheduleDto>> Handle(GetSchedulesQuery query, CancellationToken ct)
    {
        var schedules = await repository.GetByCourseIdAsync(query.CourseId, ct);
        return schedules.Select(s => new CourseScheduleDto(
            s.CourseId, s.ScheduleSerialNumber, s.ScheduleDate,
            s.StartTime, s.EndTime, s.LocationName, s.TrainerName));
    }
}
