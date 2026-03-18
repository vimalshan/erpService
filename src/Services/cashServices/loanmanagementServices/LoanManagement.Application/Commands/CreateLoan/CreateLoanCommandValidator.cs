using FluentValidation;

namespace LoanManagement.Application.Commands.CreateLoan;

public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.LoanKey)
            .NotEmpty().WithMessage("Loan key is required.")
            .MaximumLength(15).WithMessage("Loan key cannot exceed 15 characters.");

        RuleFor(x => x.OrgId)
            .GreaterThan(0).WithMessage("Organization ID must be positive.");

        RuleFor(x => x.LoanAmount)
            .GreaterThan(0).WithMessage("Loan amount must be positive.");

        RuleFor(x => x.LoanTypeId)
            .GreaterThan(0).WithMessage("Loan type is required.");

        RuleFor(x => x.BankId)
            .GreaterThan(0).WithMessage("Bank is required.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("Creator ID is required.");

        RuleFor(x => x.LoanDate)
            .NotEmpty().WithMessage("Loan date is required.");
    }
}
