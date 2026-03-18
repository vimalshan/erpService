using MediatR;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Application.Commands.ApprovePlan;

public class ApprovePlanCommandHandler : IRequestHandler<ApprovePlanCommand, bool>
{
    private readonly ILetPlanRepository _repository;
    private readonly IMessagePublisher _publisher;

    public ApprovePlanCommandHandler(ILetPlanRepository repository, IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
    }

    public async Task<bool> Handle(ApprovePlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.ReqNum, cancellationToken);
        if (plan is null) return false;

        plan.Approve(request.AppStatus, request.BhrStatus);
        await _repository.UpdateAsync(plan, cancellationToken);

        foreach (var evt in plan.DomainEvents)
            await _publisher.PublishAsync("development.events", "learning-plan.approved", evt, cancellationToken);

        plan.ClearDomainEvents();
        return true;
    }
}
