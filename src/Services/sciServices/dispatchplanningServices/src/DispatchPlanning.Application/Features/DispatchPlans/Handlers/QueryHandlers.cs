using AutoMapper;
using DispatchPlanning.Application.DTOs;
using DispatchPlanning.Application.Features.DispatchPlans.Queries;
using DispatchPlanning.Domain.Interfaces;
using MediatR;

namespace DispatchPlanning.Application.Features.DispatchPlans.Handlers;

public class GetDispatchPlanByIdHandler : IRequestHandler<GetDispatchPlanByIdQuery, DispatchPlanDetailDto?>
{
    private readonly IDispatchPlanRepository _repository;
    private readonly IMapper _mapper;

    public GetDispatchPlanByIdHandler(IDispatchPlanRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DispatchPlanDetailDto?> Handle(GetDispatchPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await _repository.GetByIdAsync(request.PlanHeaderId, cancellationToken);
        if (plan is null) return null;

        var header = _mapper.Map<DispatchPlanHeaderDto>(plan);
        var items = plan.Items.Select(i => new DispatchPlanItemDto(
            i.DispatchPlanHeaderId, i.BreakupItemId,
            i.TargetWeek1, i.TargetWeek2, i.TargetWeek3, i.TargetWeek4, i.TargetWeek5,
            i.TargetMPlus1, i.TargetMPlus2, i.TargetMPlus3, i.TargetMPlus4)).ToList();
        var sgTargets = plan.SubGroupTargets.Select(s => new DispatchPlanSubGroupTargetDto(
            s.DispatchPlanHeaderId, s.SubGroupId,
            s.TargetWeek1, s.TargetWeek2, s.TargetWeek3, s.TargetWeek4, s.TargetWeek5,
            s.TargetMPlus1, s.TargetMPlus2, s.TargetMPlus3, s.TargetMPlus4)).ToList();

        return new DispatchPlanDetailDto(header, items, sgTargets);
    }
}

public class GetAllDispatchPlansHandler : IRequestHandler<GetAllDispatchPlansQuery, IEnumerable<DispatchPlanHeaderDto>>
{
    private readonly IDispatchPlanRepository _repository;
    private readonly IMapper _mapper;

    public GetAllDispatchPlansHandler(IDispatchPlanRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DispatchPlanHeaderDto>> Handle(GetAllDispatchPlansQuery request, CancellationToken cancellationToken)
    {
        var plans = await _repository.GetAllAsync(request.CompanyUnitId, cancellationToken);
        return plans.Select(p => _mapper.Map<DispatchPlanHeaderDto>(p));
    }
}

public class GetAllMainGroupsHandler : IRequestHandler<GetAllMainGroupsQuery, IEnumerable<MainGroupDto>>
{
    private readonly IDispatchPlanMainGroupRepository _repository;
    private readonly IMapper _mapper;

    public GetAllMainGroupsHandler(IDispatchPlanMainGroupRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MainGroupDto>> Handle(GetAllMainGroupsQuery request, CancellationToken cancellationToken)
    {
        var groups = await _repository.GetAllAsync(request.CompanyUnitId, cancellationToken);
        return groups.Select(g => _mapper.Map<MainGroupDto>(g));
    }
}

public class GetSubGroupsByMainGroupHandler : IRequestHandler<GetSubGroupsByMainGroupQuery, IEnumerable<SubGroupDto>>
{
    private readonly IDispatchPlanSubGroupRepository _repository;
    private readonly IMapper _mapper;

    public GetSubGroupsByMainGroupHandler(IDispatchPlanSubGroupRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubGroupDto>> Handle(GetSubGroupsByMainGroupQuery request, CancellationToken cancellationToken)
    {
        var groups = await _repository.GetByMainGroupAsync(request.MainGroupId, cancellationToken);
        return groups.Select(g => _mapper.Map<SubGroupDto>(g));
    }
}

public class GetBreakupItemsBySubGroupHandler : IRequestHandler<GetBreakupItemsBySubGroupQuery, IEnumerable<BreakupItemDto>>
{
    private readonly IDispatchPlanBreakupItemRepository _repository;
    private readonly IMapper _mapper;

    public GetBreakupItemsBySubGroupHandler(IDispatchPlanBreakupItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BreakupItemDto>> Handle(GetBreakupItemsBySubGroupQuery request, CancellationToken cancellationToken)
    {
        var items = await _repository.GetBySubGroupAsync(request.SubGroupId, cancellationToken);
        return items.Select(i => _mapper.Map<BreakupItemDto>(i));
    }
}
