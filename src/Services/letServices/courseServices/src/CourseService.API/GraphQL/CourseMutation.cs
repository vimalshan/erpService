using CourseService.Application.Courses.Commands.CancelCourse;
using CourseService.Application.Courses.Commands.CreateCourse;
using CourseService.Application.Courses.Commands.DeleteCourse;
using CourseService.Application.DTOs;
using CourseService.Application.Participants.Commands.CancelParticipant;
using CourseService.Application.Participants.Commands.RegisterParticipant;
using CourseService.Application.Participants.Commands.UpdateAttendance;
using CourseService.Application.Schedules.Commands.CreateSchedule;
using MediatR;

namespace CourseService.API.GraphQL;

/// <summary>
/// GraphQL Mutation type - accessible at /graphql.
/// </summary>
public class CourseMutation
{
    public async Task<CourseDto> CreateCourse([Service] IMediator mediator, CreateCourseCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> CancelCourse([Service] IMediator mediator, long courseId, DateTime cancellationDate, string reason, CancellationToken ct)
        => await mediator.Send(new CancelCourseCommand(courseId, cancellationDate, reason), ct);

    public async Task<bool> DeleteCourse([Service] IMediator mediator, long courseId, CancellationToken ct)
        => await mediator.Send(new DeleteCourseCommand(courseId), ct);

    public async Task<CourseScheduleDto> CreateSchedule([Service] IMediator mediator, CreateScheduleCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<CourseParticipantDto> RegisterParticipant([Service] IMediator mediator, RegisterParticipantCommand input, CancellationToken ct)
        => await mediator.Send(input, ct);

    public async Task<bool> CancelParticipant([Service] IMediator mediator, long courseId, string userCode, DateTime cancellationDate, string remark, CancellationToken ct)
        => await mediator.Send(new CancelParticipantCommand(courseId, userCode, cancellationDate, remark), ct);

    public async Task<bool> UpdateAttendance([Service] IMediator mediator, long courseId, string userCode, char attendanceStatus, CancellationToken ct)
        => await mediator.Send(new UpdateAttendanceCommand(courseId, userCode, attendanceStatus), ct);
}
