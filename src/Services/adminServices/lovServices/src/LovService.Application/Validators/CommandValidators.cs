using FluentValidation;
using LovService.Application.Commands.LovType;
using LovService.Application.Commands.LovMaster;
using LovService.Application.Commands.ItemData;

namespace LovService.Application.Validators;

public class CreateLovTypeCommandValidator : AbstractValidator<CreateLovTypeCommand>
{
    public CreateLovTypeCommandValidator()
    {
        RuleFor(x => x.LovTypeId).GreaterThan(0);
        RuleFor(x => x.LovTypeName).NotEmpty().MaximumLength(30);
    }
}

public class CreateLovMasterCommandValidator : AbstractValidator<CreateLovMasterCommand>
{
    public CreateLovMasterCommandValidator()
    {
        RuleFor(x => x.LovId).GreaterThan(0);
        RuleFor(x => x.LovTypeId).GreaterThan(0);
        RuleFor(x => x.LovName).NotEmpty().MaximumLength(30);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class CreateItemDataCommandValidator : AbstractValidator<CreateItemDataCommand>
{
    public CreateItemDataCommandValidator()
    {
        RuleFor(x => x.CatName).MaximumLength(40);
        RuleFor(x => x.ItemName).MaximumLength(60);
        RuleFor(x => x.Make).MaximumLength(30);
        RuleFor(x => x.Uom).MaximumLength(20);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).When(x => x.Price.HasValue);
    }
}
