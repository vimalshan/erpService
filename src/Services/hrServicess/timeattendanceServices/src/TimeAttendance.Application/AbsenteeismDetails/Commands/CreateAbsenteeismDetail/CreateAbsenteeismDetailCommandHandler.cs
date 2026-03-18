using MediatR;
using TimeAttendance.Domain.Entities;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.CreateAbsenteeismDetail;

public class CreateAbsenteeismDetailCommandHandler(
    IAbsenteeismDetailRepository repository,
    IMessagePublisher messagePublisher)
    : IRequestHandler<CreateAbsenteeismDetailCommand, long>
{
    public async Task<long> Handle(
        CreateAbsenteeismDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = AbsenteeismDetail.Create(
            request.UnitId, request.Year, request.Month,
            request.TotalManDays, request.AbsentManDays,
            request.GradeCategory, request.FunctionId,
            request.AgeId, request.ExperienceId,
            request.Gender, request.InternalExperienceId,
            request.TotalExperienceId);

        await repository.AddAsync(entity, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        await messagePublisher.PublishAsync(
            "timeattendance.absenteeism.created",
            new { entity.Id, entity.UnitId, entity.Year, entity.Month },
            cancellationToken);

        return entity.Id;
    }
}
