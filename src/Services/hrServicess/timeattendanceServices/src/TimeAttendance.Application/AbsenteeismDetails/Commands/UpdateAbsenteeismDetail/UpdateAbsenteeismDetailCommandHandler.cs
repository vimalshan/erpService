using MediatR;
using TimeAttendance.Domain.Exceptions;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.UpdateAbsenteeismDetail;

public class UpdateAbsenteeismDetailCommandHandler(IAbsenteeismDetailRepository repository)
    : IRequestHandler<UpdateAbsenteeismDetailCommand, bool>
{
    public async Task<bool> Handle(
        UpdateAbsenteeismDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AbsenteeismNotFoundException(request.Id);

        entity.Update(request.TotalManDays, request.AbsentManDays, request.GradeCategory);
        repository.Update(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
