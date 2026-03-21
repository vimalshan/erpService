namespace TransactionService.Application.DTOs;

public sealed record DeptBudgetDto(
    long LocationId,
    string UnitCode,
    long DeptId,
    long FinYearId,
    long BudgetAmount,
    long UpdatedBy,
    DateTime UpdatedOn);

public sealed record UnitBudgetDto(
    long LocationId,
    string UnitCode,
    long FinYearId,
    long BudgetAmount,
    long UpdatedBy,
    DateTime UpdatedOn);

public sealed record BudgetSummaryDto(
    long LocationId,
    long DeptId,
    long FinYearId,
    long TotalBudget,
    long RemainingBudget);
