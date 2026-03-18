using FluentValidation;

namespace CanteenUnit.Application.Features.CanteenUnits.Commands.CreateCanteenUnit;

public class CreateCanteenUnitCommandValidator : AbstractValidator<CreateCanteenUnitCommand>
{
    public CreateCanteenUnitCommandValidator()
    {
        RuleFor(x => x.ComCode).GreaterThan(0).WithMessage("Company code must be a positive number.");
        RuleFor(x => x.UnitName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.UnitRef).MaximumLength(100).When(x => x.UnitRef is not null);
        RuleFor(x => x.MaxVal).GreaterThanOrEqualTo(x => x.MinVal)
            .When(x => x.MaxVal.HasValue && x.MinVal.HasValue)
            .WithMessage("Max value must be >= Min value.");
    }
}
