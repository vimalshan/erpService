using MediatR;

namespace TimeAttendance.Application.AbsenteeismMis.Commands.DeleteAbsenteeismMis;

public record DeleteAbsenteeismMisCommand(long Id) : IRequest<bool>;
