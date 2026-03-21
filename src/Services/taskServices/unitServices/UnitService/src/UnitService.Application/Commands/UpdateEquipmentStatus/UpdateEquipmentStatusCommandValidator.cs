using FluentValidation;

namespace UnitService.Application.Commands.UpdateEquipmentStatus;

public class UpdateEquipmentStatusCommandValidator : AbstractValidator<UpdateEquipmentStatusCommand>
{
    public UpdateEquipmentStatusCommandValidator()
    {
        RuleFor(x => x.StatusId).GreaterThan(0);
        RuleFor(x => x.EquipmentId).GreaterThan(0);
        RuleFor(x => x.StatusDescription).NotEmpty().MaximumLength(65);
        RuleFor(x => x.StatusCode).NotEmpty().MaximumLength(5);
        RuleFor(x => x.Remarks).MaximumLength(200);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
