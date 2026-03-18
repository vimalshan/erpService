using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Schedules.Queries.GetSchedules;

public record GetSchedulesQuery(long CourseId) : IRequest<IEnumerable<CourseScheduleDto>>;
