using MediatR;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.UpdateAbsenteeismDetail;

public record UpdateAbsenteeismDetailCommand(
    long Id,
    long TotalManDays,
    long AbsentManDays,
    string GradeCategory
) : IRequest<bool>;
