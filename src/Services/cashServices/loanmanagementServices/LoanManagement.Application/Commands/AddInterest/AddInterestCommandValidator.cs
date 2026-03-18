using FluentValidation;

namespace LoanManagement.Application.Commands.AddInterest;

public class AddInterestCommandValidator : AbstractValidator<AddInterestCommand>
{
    public AddInterestCommandValidator()
    {
        RuleFor(x => x.LoanId).GreaterThan(0).WithMessage("Loan ID is required.");
        RuleFor(x => x.RateType).Must(r => r == "FX" || r == "FL").WithMessage("Rate type must be FX or FL.");
        RuleFor(x => x.Percentage).GreaterThan(0).WithMessage("Percentage must be positive.");
        RuleFor(x => x.FloatTypeId).NotNull().When(x => x.RateType == "FL")
            .WithMessage("Float type ID is required for floating rate.");
        RuleFor(x => x.EffectiveDate).NotEmpty().WithMessage("Effective date is required.");
    }
}
