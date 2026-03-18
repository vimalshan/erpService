using CourseService.Application.DTOs;
using CourseService.Domain.Exceptions;
using CourseService.Domain.Interfaces;
using MediatR;

namespace CourseService.Application.Schedules.Commands.CreateSchedule;

public class CreateScheduleCommandHandler(ICourseRepository courseRepository, ICourseScheduleRepository scheduleRepository)
    : IRequestHandler<CreateScheduleCommand, CourseScheduleDto>
{
    public async Task<CourseScheduleDto> Handle(CreateScheduleCommand cmd, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdAsync(cmd.CourseId, ct)
            ?? throw new CourseDomainException($"Course {cmd.CourseId} not found.");

        var schedule = course.AddSchedule(
            cmd.ScheduleSerialNumber, cmd.ScheduleDate,
            cmd.StartTime, cmd.EndTime, cmd.LocationName, cmd.TrainerName);

        await scheduleRepository.AddAsync(schedule, ct);
        await courseRepository.UpdateAsync(course, ct);

        return new CourseScheduleDto(
            schedule.CourseId, schedule.ScheduleSerialNumber, schedule.ScheduleDate,
            schedule.StartTime, schedule.EndTime, schedule.LocationName, schedule.TrainerName);
    }
}
