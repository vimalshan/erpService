namespace TransactionService.Application.Queries.GetBudget;

using AutoMapper;
using MediatR;
using TransactionService.Application.DTOs;
using TransactionService.Domain.Interfaces;

public sealed class GetDeptBudgetQueryHandler : IRequestHandler<GetDeptBudgetQuery, BudgetSummaryDto?>
{
    private readonly IBudgetRepository _repository;

    public GetDeptBudgetQueryHandler(IBudgetRepository repository)
    {
        _repository = repository;
    }

    public async Task<BudgetSummaryDto?> Handle(GetDeptBudgetQuery request, CancellationToken cancellationToken)
    {
        var budget = await _repository.GetDeptBudgetAsync(
            request.LocationId, request.DeptId, request.FinYearId, cancellationToken);

        if (budget is null) return null;

        var remaining = await _repository.GetRemainingBudgetSpAsync(
            request.LocationId, request.DeptId, request.FinYearId, cancellationToken);

        return new BudgetSummaryDto(
            request.LocationId, request.DeptId, request.FinYearId,
            budget.BudgetAmount.Amount, remaining);
    }
}

public sealed class GetDeptBudgetsByLocationQueryHandler : IRequestHandler<GetDeptBudgetsByLocationQuery, IEnumerable<DeptBudgetDto>>
{
    private readonly IBudgetRepository _repository;
    private readonly IMapper _mapper;

    public GetDeptBudgetsByLocationQueryHandler(IBudgetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DeptBudgetDto>> Handle(
        GetDeptBudgetsByLocationQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _repository.GetDeptBudgetsByLocationAsync(
            request.LocationId, request.FinYearId, cancellationToken);
        return _mapper.Map<IEnumerable<DeptBudgetDto>>(budgets);
    }
}

public sealed class GetUnitBudgetsByLocationQueryHandler : IRequestHandler<GetUnitBudgetsByLocationQuery, IEnumerable<UnitBudgetDto>>
{
    private readonly IBudgetRepository _repository;
    private readonly IMapper _mapper;

    public GetUnitBudgetsByLocationQueryHandler(IBudgetRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UnitBudgetDto>> Handle(
        GetUnitBudgetsByLocationQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _repository.GetUnitBudgetsByLocationAsync(
            request.LocationId, request.FinYearId, cancellationToken);
        return _mapper.Map<IEnumerable<UnitBudgetDto>>(budgets);
    }
}
