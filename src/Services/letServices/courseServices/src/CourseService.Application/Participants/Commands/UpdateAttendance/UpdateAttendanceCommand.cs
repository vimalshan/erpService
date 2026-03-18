using MediatR;

namespace CourseService.Application.Participants.Commands.UpdateAttendance;

public record UpdateAttendanceCommand(long CourseId, string UserCode, char AttendanceStatus) : IRequest<bool>;
