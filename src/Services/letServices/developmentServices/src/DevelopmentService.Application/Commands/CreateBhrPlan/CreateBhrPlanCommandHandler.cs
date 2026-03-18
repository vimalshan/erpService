using MediatR;
using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Mappings;
using DevelopmentService.Domain.Entities;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Application.Commands.CreateBhrPlan;

public class CreateBhrPlanCommandHandler : IRequestHandler<CreateBhrPlanCommand, LetBhrPlanDto>
{
    private readonly ILetBhrPlanRepository _repository;
    private readonly IMessagePublisher _publisher;

    public CreateBhrPlanCommandHandler(
        ILetBhrPlanRepository repository,
        IMessagePublisher publisher)
    {
        _repository = repository;
        _publisher  = publisher;
    }

    public async Task<LetBhrPlanDto> Handle(CreateBhrPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = LetBhrPlan.Create(request.ReqNum, request.UserId, request.TrainingProgram,
            request.TrainingCode, request.Priority, request.BhrAccept);

        await _repository.AddAsync(plan, cancellationToken);

        foreach (var evt in plan.DomainEvents)
            await _publisher.PublishAsync("development.events", "bhr-plan.created", evt, cancellationToken);

        plan.ClearDomainEvents();
        return plan.ToDto();
    }
}
