namespace TransactionService.Application.Commands.AllocateBudget;

using MediatR;

public sealed record AllocateDeptBudgetCommand(
    long LocationId,
    string UnitCode,
    long DeptId,
    long FinYearId,
    long BudgetAmount,
    long UpdatedBy) : IRequest<bool>;

public sealed record AllocateUnitBudgetCommand(
    long LocationId,
    string UnitCode,
    long FinYearId,
    long BudgetAmount,
    long UpdatedBy) : IRequest<bool>;
