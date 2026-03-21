using ExpenseService.Application.Commands;
using FluentValidation;

namespace ExpenseService.Application.Validators;

public class RecordExpenseCommandValidator : AbstractValidator<RecordExpenseCommand>
{
    public RecordExpenseCommandValidator()
    {
        RuleFor(x => x.RequestNumber).GreaterThan(0).WithMessage("Request number must be positive.");
        RuleFor(x => x.ExpenseCode).GreaterThan(0).WithMessage("Expense code must be positive.");
        RuleFor(x => x.BudgetAmount).GreaterThanOrEqualTo(0).WithMessage("Budget amount cannot be negative.");
        RuleFor(x => x.EligibleAmount).GreaterThanOrEqualTo(0).WithMessage("Eligible amount cannot be negative.");
        RuleFor(x => x.CompanyAmount).GreaterThanOrEqualTo(0).WithMessage("Company amount cannot be negative.");
        RuleFor(x => x.SelfAmount).GreaterThanOrEqualTo(0).WithMessage("Self amount cannot be negative.");
        RuleFor(x => x)
            .Must(x => x.CompanyAmount + x.SelfAmount == x.EligibleAmount)
            .WithMessage("Company and Self amounts must equal the eligible amount.");
    }
}
