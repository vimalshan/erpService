using CourseService.Application.DTOs;
using MediatR;

namespace CourseService.Application.Schedules.Commands.CreateSchedule;

public record CreateScheduleCommand(
    long CourseId,
    long ScheduleSerialNumber,
    DateTime ScheduleDate,
    string StartTime,
    string EndTime,
    string LocationName,
    string TrainerName
) : IRequest<CourseScheduleDto>;
