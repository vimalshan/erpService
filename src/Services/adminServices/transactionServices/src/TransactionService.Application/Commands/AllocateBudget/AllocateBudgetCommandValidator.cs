namespace TransactionService.Application.Commands.AllocateBudget;

using FluentValidation;

public sealed class AllocateDeptBudgetCommandValidator : AbstractValidator<AllocateDeptBudgetCommand>
{
    public AllocateDeptBudgetCommandValidator()
    {
        RuleFor(x => x.LocationId).GreaterThan(0);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.DeptId).GreaterThan(0);
        RuleFor(x => x.FinYearId).GreaterThan(0);
        RuleFor(x => x.BudgetAmount).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public sealed class AllocateUnitBudgetCommandValidator : AbstractValidator<AllocateUnitBudgetCommand>
{
    public AllocateUnitBudgetCommandValidator()
    {
        RuleFor(x => x.LocationId).GreaterThan(0);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.FinYearId).GreaterThan(0);
        RuleFor(x => x.BudgetAmount).GreaterThan(0);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}
