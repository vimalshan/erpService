using FluentValidation;

namespace LoanManagement.Application.Commands.AddDisbursement;

public class AddDisbursementCommandValidator : AbstractValidator<AddDisbursementCommand>
{
    public AddDisbursementCommandValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0).WithMessage("Loan ID is required.");
        RuleFor(x => x.DisbDate).NotEmpty().WithMessage("Disbursement date is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Disbursement amount must be positive.");
        RuleFor(x => x.ExcRate).GreaterThan(0).When(x => x.ExcRate.HasValue)
            .WithMessage("Exchange rate must be positive.");
    }
}
