using FluentValidation;

namespace UnitService.Application.Commands.RegisterEquipment;

public class RegisterEquipmentCommandValidator : AbstractValidator<RegisterEquipmentCommand>
{
    public RegisterEquipmentCommandValidator()
    {
        RuleFor(x => x.EquipmentId).GreaterThan(0);
        RuleFor(x => x.EquipmentName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(25);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
