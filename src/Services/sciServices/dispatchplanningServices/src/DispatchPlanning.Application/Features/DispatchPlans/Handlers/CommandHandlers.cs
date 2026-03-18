using AutoMapper;
using DispatchPlanning.Application.DTOs;
using DispatchPlanning.Application.Features.DispatchPlans.Commands;
using DispatchPlanning.Domain.Aggregates;
using DispatchPlanning.Domain.Interfaces;
using DispatchPlanning.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DispatchPlanning.Application.Features.DispatchPlans.Handlers;

public class CreateDispatchPlanHandler : IRequestHandler<CreateDispatchPlanCommand, int>
{
    private readonly IDispatchPlanRepository _repository;
    private readonly ILogger<CreateDispatchPlanHandler> _logger;

    public CreateDispatchPlanHandler(IDispatchPlanRepository repository,
        ILogger<CreateDispatchPlanHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<int> Handle(CreateDispatchPlanCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating dispatch plan: Type={Type}, Month={Month}, CompanyUnit={CompanyUnit}",
            request.PlanType, request.PlanMonth, request.CompanyUnitId);

        var aggregate = DispatchPlanAggregate.Create(
            0,
            request.PlanType,
            request.PlanMonth,
            request.CompanyUnitId,
            request.ModifiedBy,
            request.MPlus1,
            request.MPlus2,
            request.MPlus3,
            request.MPlus4);

        var id = await _repository.AddAsync(aggregate, cancellationToken);
        _logger.LogInformation("Dispatch plan created with ID={Id}", id);
        return id;
    }
}

public class AddDispatchPlanItemHandler : IRequestHandler<AddDispatchPlanItemCommand, Unit>
{
    private readonly IDispatchPlanRepository _repository;

    public AddDispatchPlanItemHandler(IDispatchPlanRepository repository) => _repository = repository;

    public async Task<Unit> Handle(AddDispatchPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.PlanHeaderId, cancellationToken)
            ?? throw new Domain.Exceptions.DispatchPlanNotFoundException(request.PlanHeaderId);

        var targets = new TargetWeeks(request.TargetWeek1, request.TargetWeek2, request.TargetWeek3,
            request.TargetWeek4, request.TargetWeek5,
            request.TargetMPlus1, request.TargetMPlus2, request.TargetMPlus3, request.TargetMPlus4);

        plan.AddItemTarget(request.BreakupItemId, targets, request.ModifiedBy);
        await _repository.UpdateAsync(plan, cancellationToken);
        return Unit.Value;
    }
}

public class UpdateDispatchPlanForecastHandler : IRequestHandler<UpdateDispatchPlanForecastCommand, Unit>
{
    private readonly IDispatchPlanRepository _repository;

    public UpdateDispatchPlanForecastHandler(IDispatchPlanRepository repository) => _repository = repository;

    public async Task<Unit> Handle(UpdateDispatchPlanForecastCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.PlanHeaderId, cancellationToken)
            ?? throw new Domain.Exceptions.DispatchPlanNotFoundException(request.PlanHeaderId);

        plan.UpdatePlanForecasts(request.MPlus1, request.MPlus2, request.MPlus3, request.MPlus4, request.ModifiedBy);
        await _repository.UpdateAsync(plan, cancellationToken);
        return Unit.Value;
    }
}

public class DeleteDispatchPlanHandler : IRequestHandler<DeleteDispatchPlanCommand, Unit>
{
    private readonly IDispatchPlanRepository _repository;

    public DeleteDispatchPlanHandler(IDispatchPlanRepository repository) => _repository = repository;

    public async Task<Unit> Handle(DeleteDispatchPlanCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.PlanHeaderId, cancellationToken);
        return Unit.Value;
    }
}

public class AddSubGroupTargetHandler : IRequestHandler<AddSubGroupTargetCommand, Unit>
{
    private readonly IDispatchPlanRepository _repository;

    public AddSubGroupTargetHandler(IDispatchPlanRepository repository) => _repository = repository;

    public async Task<Unit> Handle(AddSubGroupTargetCommand request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.PlanHeaderId, cancellationToken)
            ?? throw new Domain.Exceptions.DispatchPlanNotFoundException(request.PlanHeaderId);

        var targets = new TargetWeeks(request.TargetWeek1, request.TargetWeek2, request.TargetWeek3,
            request.TargetWeek4, request.TargetWeek5,
            request.TargetMPlus1, request.TargetMPlus2, request.TargetMPlus3, request.TargetMPlus4);

        plan.AddSubGroupTarget(request.SubGroupId, targets, request.ModifiedBy);
        await _repository.UpdateAsync(plan, cancellationToken);
        return Unit.Value;
    }
}
