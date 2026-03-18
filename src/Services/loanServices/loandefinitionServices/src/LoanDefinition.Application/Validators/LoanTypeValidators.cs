using FluentValidation;
using LoanDefinition.Application.Features.LoanTypes.Commands;

namespace LoanDefinition.Application.Validators;

public class CreateLoanTypeCommandValidator : AbstractValidator<CreateLoanTypeCommand>
{
    public CreateLoanTypeCommandValidator()
    {
        RuleFor(x => x.LoanType).GreaterThan(0);
        RuleFor(x => x.LoanName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LoanCategory).NotEmpty().MaximumLength(10);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class UpdateLoanTypeCommandValidator : AbstractValidator<UpdateLoanTypeCommand>
{
    public UpdateLoanTypeCommandValidator()
    {
        RuleFor(x => x.LoanType).GreaterThan(0);
        RuleFor(x => x.LoanName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LoanCategory).NotEmpty().MaximumLength(10);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
