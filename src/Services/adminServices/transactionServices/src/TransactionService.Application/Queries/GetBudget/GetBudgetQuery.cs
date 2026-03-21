namespace TransactionService.Application.Queries.GetBudget;

using MediatR;
using TransactionService.Application.DTOs;

public sealed record GetDeptBudgetQuery(
    long LocationId, long DeptId, long FinYearId) : IRequest<BudgetSummaryDto?>;

public sealed record GetDeptBudgetsByLocationQuery(
    long LocationId, long FinYearId) : IRequest<IEnumerable<DeptBudgetDto>>;

public sealed record GetUnitBudgetsByLocationQuery(
    long LocationId, long FinYearId) : IRequest<IEnumerable<UnitBudgetDto>>;
