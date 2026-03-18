using MediatR;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.DeleteAbsenteeismDetail;

public record DeleteAbsenteeismDetailCommand(long Id) : IRequest<bool>;
