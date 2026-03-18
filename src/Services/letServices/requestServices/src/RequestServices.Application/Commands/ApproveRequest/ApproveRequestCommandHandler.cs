using MediatR;
using RequestServices.Application.Interfaces;
using RequestServices.Domain.Exceptions;
using RequestServices.Domain.Interfaces;

namespace RequestServices.Application.Commands.ApproveRequest;

public class ApproveRequestCommandHandler(
    IRequestRepository repository,
    IUnitOfWork unitOfWork,
    IDomainEventDispatcher eventDispatcher)
    : IRequestHandler<ApproveRequestCommand, bool>
{
    public async Task<bool> Handle(ApproveRequestCommand cmd, CancellationToken ct)
    {
        var main = await repository.GetByIdAsync(cmd.RequestId, ct)
            ?? throw new RequestNotFoundException(cmd.RequestId);

        // Re-hydrate into aggregate to invoke business logic
        var aggregate = RequestServices.Domain.Aggregates.RequestAggregate.Create(
            main.RequestId, main.EmployeeUser, main.RequestDate, main.SupervisorUser);

        foreach (var sub in main.SubRequests)
            aggregate.AddSubRequest(sub);

        aggregate.Approve(cmd.SerialNumber, cmd.ApprovalNumber, cmd.ApprovalRemark, cmd.ApprovalUser);

        await repository.UpdateAsync(aggregate, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await eventDispatcher.DispatchAsync(aggregate.DomainEvents, ct);
        aggregate.ClearDomainEvents();

        return true;
    }
}
