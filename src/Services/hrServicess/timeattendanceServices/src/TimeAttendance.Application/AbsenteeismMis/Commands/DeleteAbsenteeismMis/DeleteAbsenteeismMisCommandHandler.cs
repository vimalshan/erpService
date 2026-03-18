using MediatR;
using TimeAttendance.Domain.Exceptions;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Application.AbsenteeismMis.Commands.DeleteAbsenteeismMis;

public class DeleteAbsenteeismMisCommandHandler(IAbsenteeismMisRepository repository)
    : IRequestHandler<DeleteAbsenteeismMisCommand, bool>
{
    public async Task<bool> Handle(
        DeleteAbsenteeismMisCommand request, CancellationToken cancellationToken)
    {
        var entity = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new AbsenteeismMisNotFoundException(request.Id);

        repository.Remove(entity);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
