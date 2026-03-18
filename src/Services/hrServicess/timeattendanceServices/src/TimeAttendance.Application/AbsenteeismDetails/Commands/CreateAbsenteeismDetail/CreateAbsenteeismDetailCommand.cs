using MediatR;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.CreateAbsenteeismDetail;

public record CreateAbsenteeismDetailCommand(
    long UnitId,
    int Year,
    int Month,
    long TotalManDays,
    long AbsentManDays,
    string GradeCategory,
    long FunctionId,
    long AgeId,
    long ExperienceId,
    char Gender,
    long InternalExperienceId,
    long TotalExperienceId
) : IRequest<long>;
