using FluentValidation;
using ItemMasterService.Application.CQRS.Commands;

namespace ItemMasterService.Application.Validators;

public class CreateCanteenItemValidator : AbstractValidator<CreateCanteenItemCommand>
{
    public CreateCanteenItemValidator()
    {
        RuleFor(x => x.CanteenUnitCode).GreaterThan(0).WithMessage("Canteen unit code must be positive.");
        RuleFor(x => x.ItemCode).GreaterThan(0).WithMessage("Item code must be positive.");
        RuleFor(x => x.ItemDescription).MaximumLength(50).When(x => x.ItemDescription is not null);
        RuleFor(x => x.ItemType).MaximumLength(1).When(x => x.ItemType is not null);
        RuleFor(x => x.ItemReference).MaximumLength(10).When(x => x.ItemReference is not null);
        RuleFor(x => x.EnteredBy).NotEmpty().MaximumLength(50);
    }
}

public class UpdateCanteenItemValidator : AbstractValidator<UpdateCanteenItemCommand>
{
    public UpdateCanteenItemValidator()
    {
        RuleFor(x => x.CanteenUnitCode).GreaterThan(0);
        RuleFor(x => x.ItemCode).GreaterThan(0);
        RuleFor(x => x.ItemDescription).MaximumLength(50).When(x => x.ItemDescription is not null);
        RuleFor(x => x.ItemType).MaximumLength(1).When(x => x.ItemType is not null);
        RuleFor(x => x.ItemReference).MaximumLength(10).When(x => x.ItemReference is not null);
    }
}

public class CreateItemPriceValidator : AbstractValidator<CreateItemPriceCommand>
{
    public CreateItemPriceValidator()
    {
        RuleFor(x => x.CanteenUnitCode).GreaterThan(0);
        RuleFor(x => x.ItemCode).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.EmployeeContribution).GreaterThanOrEqualTo(0).When(x => x.EmployeeContribution.HasValue);
        RuleFor(x => x.EmployerContribution).GreaterThanOrEqualTo(0).When(x => x.EmployerContribution.HasValue);
        RuleFor(x => x.EnteredBy).NotEmpty().MaximumLength(50);
    }
}

public class CreateGradeItemPriceValidator : AbstractValidator<CreateGradeItemPriceCommand>
{
    public CreateGradeItemPriceValidator()
    {
        RuleFor(x => x.CanteenUnitCode).GreaterThan(0);
        RuleFor(x => x.GradeType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.EnteredBy).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ClosureDate).NotEmpty();
    }
}
