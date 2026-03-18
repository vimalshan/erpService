using MediatR;
using RequestServices.Application.Interfaces;
using RequestServices.Domain.Exceptions;
using RequestServices.Domain.Interfaces;

namespace RequestServices.Application.Commands.CancelRequest;

public class CancelRequestCommandHandler(
    IRequestRepository repository,
    IUnitOfWork unitOfWork,
    IDomainEventDispatcher eventDispatcher)
    : IRequestHandler<CancelRequestCommand, bool>
{
    public async Task<bool> Handle(CancelRequestCommand cmd, CancellationToken ct)
    {
        var main = await repository.GetByIdAsync(cmd.RequestId, ct)
            ?? throw new RequestNotFoundException(cmd.RequestId);

        var aggregate = Domain.Aggregates.RequestAggregate.Create(
            main.RequestId, main.EmployeeUser, main.RequestDate, main.SupervisorUser);

        foreach (var sub in main.SubRequests)
            aggregate.AddSubRequest(sub);

        aggregate.Cancel(cmd.SerialNumber, cmd.Remark);

        await repository.UpdateAsync(aggregate, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await eventDispatcher.DispatchAsync(aggregate.DomainEvents, ct);
        aggregate.ClearDomainEvents();

        return true;
    }
}
