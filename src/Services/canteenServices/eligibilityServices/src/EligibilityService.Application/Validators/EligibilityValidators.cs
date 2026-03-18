using FluentValidation;
using EligibilityService.Application.Commands.EligibilityMaster;

namespace EligibilityService.Application.Validators;

public class CreateEligibilityMasterValidator : AbstractValidator<CreateEligibilityMasterCommand>
{
    public CreateEligibilityMasterValidator()
    {
        RuleFor(x => x.CanteenUnit).GreaterThan(0).WithMessage("Canteen unit must be positive.");
        RuleFor(x => x.ShiftCode).NotEmpty().MaximumLength(1).WithMessage("Shift code must be a single character.");
        RuleFor(x => x.ItemCode).GreaterThan(0).WithMessage("Item code must be positive.");
        RuleFor(x => x.EligibleLimit).GreaterThanOrEqualTo(0).When(x => x.EligibleLimit.HasValue);
        RuleFor(x => x.TimeOfficeUnit).MaximumLength(3).When(x => x.TimeOfficeUnit is not null);
    }
}

public class UpdateEligibilityMasterValidator : AbstractValidator<UpdateEligibilityMasterCommand>
{
    public UpdateEligibilityMasterValidator()
    {
        RuleFor(x => x.CanteenUnit).GreaterThan(0);
        RuleFor(x => x.ShiftCode).NotEmpty().MaximumLength(1);
        RuleFor(x => x.ItemCode).GreaterThan(0);
        RuleFor(x => x.ModifiedUser).GreaterThan(0).WithMessage("Modified user is required.");
        RuleFor(x => x.EligibleLimit).GreaterThanOrEqualTo(0).When(x => x.EligibleLimit.HasValue);
    }
}
