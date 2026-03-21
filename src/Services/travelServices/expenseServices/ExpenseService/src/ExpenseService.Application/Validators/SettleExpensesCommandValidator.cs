using ExpenseService.Application.Commands;
using FluentValidation;

namespace ExpenseService.Application.Validators;

public class SettleExpensesCommandValidator : AbstractValidator<SettleExpensesCommand>
{
    public SettleExpensesCommandValidator()
    {
        RuleFor(x => x.RequestNumber).GreaterThan(0).WithMessage("Request number must be positive.");
    }
}
