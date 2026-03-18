using MediatR;
using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Mappings;
using DevelopmentService.Domain.Aggregates;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Application.Commands.CreateLearningPlan;

public class CreateLearningPlanCommandHandler : IRequestHandler<CreateLearningPlanCommand, LetPlanDto>
{
    private readonly ILetPlanRepository _repository;
    private readonly IMessagePublisher _publisher;

    public CreateLearningPlanCommandHandler(
        ILetPlanRepository repository,
        IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
    }

    public async Task<LetPlanDto> Handle(CreateLearningPlanCommand request, CancellationToken cancellationToken)
    {
        var aggregate = DevelopmentPlanAggregate.CreateNew(
            request.ReqNum, request.UserId, request.PinNum,
            request.DevSource, request.DevNeed, request.Priority, request.EntDate);

        await _repository.AddAsync(aggregate.Plan, cancellationToken);

        foreach (var evt in aggregate.DomainEvents)
            await _publisher.PublishAsync("development.events", "learning-plan.created", evt, cancellationToken);

        aggregate.ClearDomainEvents();
        return aggregate.Plan.ToDto();
    }
}
