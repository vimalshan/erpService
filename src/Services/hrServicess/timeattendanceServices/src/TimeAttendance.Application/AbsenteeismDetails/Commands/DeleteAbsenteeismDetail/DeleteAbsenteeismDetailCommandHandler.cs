using MediatR;
using TimeAttendance.Domain.Exceptions;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismDetails.Commands.DeleteAbsenteeismDetail;

public class DeleteAbsenteeismDetailCommandHandler(IAbsenteeismDetailRepository repository)
    : IRequestHandler<DeleteAbsenteeismDetailCommand, bool>
{
    public async Task<bool> Handle(
        DeleteAbsenteeismDetailCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AbsenteeismNotFoundException(request.Id);

        repository.Remove(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
