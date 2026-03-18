using FluentValidation;
using LoanDefinition.Application.Features.Loans.Commands;

namespace LoanDefinition.Application.Validators;

public class CreateLoanCommandValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanCommandValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0);
        RuleFor(x => x.LoanName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.LoanPurpose).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LoanTypeId).GreaterThan(0);
        RuleFor(x => x.MinimumLimit).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaximumLimit).GreaterThanOrEqualTo(x => x.MinimumLimit);
        RuleFor(x => x.RecoveryType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.CompoundingFactor).NotEmpty().MaximumLength(1);
        RuleFor(x => x.InterestFrequency).NotEmpty().MaximumLength(1);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class UpdateLoanCommandValidator : AbstractValidator<UpdateLoanCommand>
{
    public UpdateLoanCommandValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0);
        RuleFor(x => x.LoanName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.LoanPurpose).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MinimumLimit).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MaximumLimit).GreaterThanOrEqualTo(x => x.MinimumLimit);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
